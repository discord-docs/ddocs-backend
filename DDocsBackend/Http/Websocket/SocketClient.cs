using DDocsBackend.Data.Models;
using DDocsBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public class SocketClient
    {
        public const int HearbeatInterval = 31000;
        public string CurrentPage { get; private set; }

        public event Func<Packet, Task>? PayloadReceived
        {
            add => _payloadListeners.Add(value);
            remove => _payloadListeners.Remove(value);
        }

        public event Func<Task>? OnDisconnect
        {
            add => _disconnectListeners.Add(value);
            remove => _disconnectListeners.Remove(value);
        }

        public EventTypes Events { get; private set; }
        public ulong UserId { get; private set; }
        public bool IsConnected { get; private set; }

        private readonly WebSocket _socket;
        private readonly List<Func<Packet, Task>?> _payloadListeners = new();
        private readonly List<Func<Task>?> _disconnectListeners = new();
        private readonly Task _receiveLoop;
        private readonly Task _heartbeatLoop;
        private readonly Logger _log;
        private CancellationTokenSource _cancelToken;
        private TaskCompletionSource _heartbeatReceived = new();
        private readonly WebsocketServer _server;

        public SocketClient(Authentication auth, Identity identity, WebSocket socket, WebsocketServer server)
        {
            Events = identity.Events;
            CurrentPage = identity.Page!;
            UserId = auth.UserId;
            _log = Logger.GetLogger<SocketClient>();
            _socket = socket;
            _cancelToken = new();
            _server = server;

            // start heartbeat and listen loop
            _receiveLoop = Task.Run(async () => await ReceiveAsync());
            _heartbeatLoop = Task.Run(async () => await HeartbeatAsync());
            IsConnected = true;
        }

        public async Task SendAsync(Packet packet)
        {
            // encode
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet));

            // send it away
            await _socket.SendAsync(bytes, WebSocketMessageType.Text, true, _cancelToken.Token).ConfigureAwait(false);
        }

        private async Task ReceiveAsync()
        {
            try
            {
                while (IsConnected)
                {
                    var result = await GetPacketAsync(_cancelToken.Token).ConfigureAwait(false);

                    if (!result.HasValue)
                    {
                        _log.Warn($"Got payload over 2kb from {UserId}", Severity.Socket);
                        return; // return because if the result is null that means we've closed the socket so no need to listen anymore.
                    }

                    if (result.Value.Result != null)
                    {
                        _log.Warn($"Got invalid payload from {UserId}", Severity.Socket);
                        return; // return because if the WebSocketReceiveResult is not null that means we've closed the socket so no need to listen anymore.
                    }

                    // decode the payload
                    Packet? packet = null;
                    try
                    {
                        string json = Encoding.UTF8.GetString(result.Value.Data.ToArray());

                        packet = JsonConvert.DeserializeObject<Packet>(json);
                    }
                    catch (Exception x)
                    {
                        _log.Error($"Failed to parse packet content from {UserId}", Severity.Socket, x);
                        await DisconnectAsync(WebSocketCloseStatus.InvalidPayloadData).ConfigureAwait(false);
                        return;
                    }

                    if (packet == null)
                    {
                        _log.Warn($"Got null packet from {UserId}", Severity.Socket);
                        await DisconnectAsync(WebSocketCloseStatus.InvalidPayloadData).ConfigureAwait(false);
                        return;
                    }

                    _ = Task.Run(async () => await HandlePacketAsync(packet));
                }
            }
            catch (OperationCanceledException) { }
            catch(Exception x)
            {
                _log.Critical("Failed in read loop for socket", Severity.Socket, x);
            }
        }

        private async Task HandlePacketAsync(Packet p)
        {
            _log.Debug($"Got op {p.Type} from {UserId}", Severity.Socket);
            switch (p.Type)
            {
                case PacketType.Heartbeat:
                    {
                        _heartbeatReceived.TrySetResult();
                        await SendAsync(new Packet
                        {
                            Type = PacketType.HeartbeatAck
                        });
                    }
                    break;
                case PacketType.SwitchingPage:
                    {
                        var payload = p.PayloadAs<PageSwitch>();
                        if (payload == null || payload.Page == null)
                            return;

                        CurrentPage = payload.Page;

                        _log.Info($"{UserId} is now on {CurrentPage}", Severity.Socket);
                    }
                    break;
                case PacketType.GetUsers:
                    {
                        var otherUsers = _server.ConnectedClients.Where(x => x.UserId != UserId && x.CurrentPage == CurrentPage);

                        await SendAsync(new Packet
                        {
                            Type = PacketType.Users,
                            Payload = new UsersResult
                            {
                                Users = await Task.WhenAll(otherUsers.Select(x => x.ToAuthorAsync())).ConfigureAwait(false)
                            }
                        });
                    }
                    break;
                default:
                    InvokePayloadListeners(p);
                    break;
            }
        }

        public async Task HeartbeatAsync()
        {
            while (IsConnected)
            {
                await Task.WhenAny(_heartbeatReceived.Task, Task.Delay(HearbeatInterval));

                if (_cancelToken.IsCancellationRequested)
                    return;

                if (!_heartbeatReceived.Task.IsCompleted)
                {
                    // failed heartbeat
                    await DisconnectAsync(WebSocketCloseStatus.PolicyViolation, "Failed heartbeat");
                }

                _heartbeatReceived = new();
            }
        }

        private async Task<(WebSocketReceiveResult? Result, List<byte> Data)?> GetPacketAsync(CancellationToken token = default)
        {
            List<byte> packet = new();
            bool isEnd = false;

            try
            {
                while (!isEnd && packet.Count < WebsocketServer.MaxPacketSize)
                {
                    var buff = new byte[512];
                    WebSocketReceiveResult result = await _socket.ReceiveAsync(buff, token).ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        await DisconnectAsync(WebSocketCloseStatus.InvalidPayloadData, null, default).ConfigureAwait(false);
                        return (result, packet);
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectAsync(WebSocketCloseStatus.NormalClosure, null, default).ConfigureAwait(false);
                        return (result, packet);
                    }

                    isEnd = result.EndOfMessage;
                    packet.AddRange(buff.Take(result.Count));
                }
            }
            catch (Exception x) when (x.GetType() != typeof(OperationCanceledException) || x.GetType() != typeof(TaskCanceledException))
            {
                _log.Error("Failed to read from socket", Severity.Socket, x);
                return null;
            }

            if (packet.Count > WebsocketServer.MaxPacketSize)
            {
                await DisconnectAsync(WebSocketCloseStatus.MessageTooBig, null, default).ConfigureAwait(false);
                return null;
            }

            return (null, packet);
        }

        public async Task DisconnectAsync(WebSocketCloseStatus status = WebSocketCloseStatus.NormalClosure, string? message = null, CancellationToken token = default)
        {
            IsConnected = false;
            await _socket.CloseOutputAsync(status, message, token).ConfigureAwait(false);
            _cancelToken.Cancel();
            InvokeDisconnectListeners();
            try
            {
                _heartbeatLoop.Dispose();
                _receiveLoop.Dispose();
            }
            catch{ }
        }

        private void InvokePayloadListeners(Packet p)
        {
            foreach(var t in _payloadListeners.ToArray())
            {
                if (t == null)
                    continue;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        var task = t(p);

                        await task;

                        if(task.Exception != null)
                        {
                            _log.Warn("Failed to invoke payload listener", Severity.Socket, task.Exception);
                        }
                    }
                    catch (Exception x)
                    {
                        _log.Warn("Failed to invoke payload listener", Severity.Socket, x);
                    }
                });
            }
        }

        private void InvokeDisconnectListeners()
        {
            foreach (var t in _disconnectListeners.ToArray())
            {
                if (t == null)
                    continue;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        var task = t();

                        await task;

                        if (task.Exception != null)
                        {
                            _log.Warn("Failed to invoke payload listener", Severity.Socket, task.Exception);
                        }
                    }
                    catch (Exception x)
                    {
                        _log.Warn("Failed to invoke payload listener", Severity.Socket, x);
                    }
                });
            }
        }

        private async Task<RestModels.Author> ToAuthorAsync()
        {
            var bridgeService = _server.Server.Provider.GetRequiredService<DiscordBridgeService>();

            var user = await bridgeService.GetUserDetailsAsync(UserId).ConfigureAwait(false);

            return new RestModels.Author
            {
                Avatar = user.Avatar,
                Discriminator = user.Discriminator,
                UserId = UserId,
                Username = user.Username
            };
        }
    }
}
