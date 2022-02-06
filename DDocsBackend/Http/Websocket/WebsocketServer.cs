using DDocsBackend.Data;
using DDocsBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Http.Websocket
{
    public class WebsocketServer
    {
        public const EventTypes RestrictedType =
            EventTypes.DraftCreated |
            EventTypes.DraftModified |
            EventTypes.SummaryCreated |
            EventTypes.SummaryDeleted |
            EventTypes.SummaryModifed;

        public const int IdentityTimeout = 5000;
        public const int MaxPacketSize = 2048;

        private readonly HttpServer _server;
        private readonly Logger _log;

        private readonly List<SocketClient> _connectedClients = new();

        private AuthenticationService _authenticationService
            => _server.Provider.GetRequiredService<AuthenticationService>();

        private DataAccessLayer _dataAccessLayer
            => _server.Provider.GetRequiredService<DataAccessLayer>();

        public WebsocketServer(HttpServer server)
        {
            _log = Logger.GetLogger<WebsocketServer>();
            _server = server;
        }

        public int DispatchEvent(EventTypes type, object payload)
        {
            var packet = new Packet
            {
                Type = PacketType.Event,
                Payload = new Event
                {
                    Type = type,
                    Payload = payload
                }
            };

            var clients = _connectedClients.Where(x => x.IsConnected && x.Events.HasFlag(type));

            _ = Task.Run(async () => await Task.WhenAll(clients.Select(x => x.SendAsync(packet))));

            return clients.Count();
        }

        public async Task<bool> TryAcceptSocketAsync(HttpListenerContext context)
        {
            HttpListenerWebSocketContext? socket = null;
            try
            {
                socket = await context.AcceptWebSocketAsync(null).ConfigureAwait(false);

                if (socket == null)
                    return false;

                // wait for identity
                var cts = new CancellationTokenSource();
                cts.CancelAfter(IdentityTimeout);

                Packet? packet;
                try
                {
                    packet = await ReceiveAsync(socket.WebSocket, cts.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    _log.Warn("Client connected but didn't semd handshake", Severity.Socket);
                    await socket.WebSocket.CloseAsync(WebSocketCloseStatus.ProtocolError, "No handshake", default).ConfigureAwait(false);
                    return false;
                }

                if(packet?.Type != PacketType.Identify)
                {
                    _log.Warn("Client connected but didn't send correct handshake", Severity.Socket);
                    await socket.WebSocket.CloseAsync(WebSocketCloseStatus.ProtocolError, "No handshake", default).ConfigureAwait(false);
                    return false;
                }

                // authorize the user
                var identity = packet.PayloadAs<Identity>();

                if(identity == null || !identity.Validate())
                {
                    _log.Warn("Client connected but didn't send valid handshake", Severity.Socket);
                    await socket.WebSocket.CloseAsync(WebSocketCloseStatus.ProtocolError, "No handshake", default).ConfigureAwait(false);
                    return false;
                }

                var authentication = await _authenticationService.GetAuthenticationAsync(identity.Token!).ConfigureAwait(false);

                if(authentication == null)
                {
                    _log.Warn("Client connected but didn't send valid auth in handshake", Severity.Socket);
                    await socket.WebSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid auth", default).ConfigureAwait(false);
                    return false;
                }

                // check the requested events

                var isAuthor = await _dataAccessLayer.IsAuthorAsync(authentication.UserId);

                if(identity.Events.HasFlag(RestrictedType) && !isAuthor)
                {
                    _log.Warn("Client requested events it didnt have access for", Severity.Socket);
                    await socket.WebSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid events", default).ConfigureAwait(false);
                    return false;
                }

                var client = new SocketClient(authentication, identity.Events, socket.WebSocket);

                _connectedClients.Add(client);

                client.OnDisconnect += () =>
                {
                    _connectedClients.Remove(client);
                    return Task.CompletedTask;
                };

                return true;

            }
            catch(Exception x)
            {
                _log.Error("Failed to accept websocket", Severity.Socket, x);
                await (socket?.WebSocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, null, default) ?? Task.CompletedTask);
                return false;
            }
        }

        private async Task<Packet?> ReceiveAsync(WebSocket socket, CancellationToken token = default)
        {
            var result = await GetPacketAsync(socket, token).ConfigureAwait(false);

            if (!result.HasValue)
                return null;

            if(result.Value.Result != null)
            {
                _log.Warn($"Packet receiving failed for unknown cause", Severity.Socket);
                return null;
            }

            var json = Encoding.UTF8.GetString(result.Value.Data.ToArray());

            return JsonConvert.DeserializeObject<Packet?>(json);
        }

        private async Task<(WebSocketReceiveResult? Result, List<byte> Data)?> GetPacketAsync(WebSocket socket, CancellationToken token = default)
        {
            List<byte> packet = new();
            bool isEnd = false;

            while (!isEnd && packet.Count < MaxPacketSize)
            {
                var buff = new byte[512];
                WebSocketReceiveResult result = await socket.ReceiveAsync(buff, token).ConfigureAwait(false);

                if(result.MessageType == WebSocketMessageType.Binary)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, null, default).ConfigureAwait(false);
                    return (result, packet);
                }

                if(result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, default).ConfigureAwait(false);
                    return (result, packet);
                }

                isEnd = result.EndOfMessage;
                packet.AddRange(buff.Take(result.Count));
            }

            if(packet.Count < MaxPacketSize)
            {
                await socket.CloseAsync(WebSocketCloseStatus.MessageTooBig, null, default).ConfigureAwait(false);
                return null;
            }

            return (null, packet);
        }
    }
}
