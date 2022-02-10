using DDocsBackend.Data;
using DDocsBackend.Http.Websocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net;

namespace DDocsBackend;

public class HttpServer : IHostedService
{
    internal readonly IServiceProvider Provider;
    internal readonly WebsocketServer WebsocketServer;

    private readonly HttpListener _listener;
    private readonly HttpRestHandler _handler;
    private readonly Logger _log;
    private readonly int _port;
    public HttpServer(IConfiguration config, DataAccessLayer dataAccessLayer, IServiceProvider provider)
    {
        _log = Logger.GetLogger<HttpServer>();

        Provider = provider;

        var rawPort = config["PORT"];
        var port = 8080;

        if(rawPort == null)
        {
            _log.Warn("No http port found in the config, using 8080 as default");
        }
        else if(!int.TryParse(rawPort, out port))
        {
            _log.Error("Port env variable was not a number!");
        }

        _log.Info("Creating HTTP Server...", Severity.Rest);
        _listener = new HttpListener();

#if DEBUG
        _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        _listener.Prefixes.Add($"http://localhost:{port}/");
#else
        _listener.Prefixes.Add($"http://*:{port}/");
#endif

        _handler = new(this);

        WebsocketServer = new WebsocketServer(this);

        _port = port;
    }


    private async Task HandleRequest()
    {
        while (_listener.IsListening)
        {
            var context = await _listener.GetContextAsync().ConfigureAwait(false);
            _ = Task.Run(async () =>
            {
                await HandleContext(context).ConfigureAwait(false);
            });
        }
    }

    private async Task HandleContext(HttpListenerContext context)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var code = await _handler.ProcessRestRequestAsync(context);
            sw.Stop();
            _log.Trace($"{sw.ElapsedMilliseconds}ms: {GetColorFromMethod(context.Request.HttpMethod)} => {context.Request.RawUrl} {code}", Severity.Rest);
        }
        catch (Exception x)
        {
            _log.Critical($"Uncaught exception in handler: {x}", Severity.Rest);
        }
    }

    private string GetColorFromMethod(string method)
    {
        switch (method)
        {
            case "GET":
                return Logger.BuildColoredString(method, ConsoleColor.Green);
            case "POST":
                return Logger.BuildColoredString(method, ConsoleColor.DarkYellow);
            case "PUT":
                return Logger.BuildColoredString(method, ConsoleColor.Blue);
            case "DELETE":
                return Logger.BuildColoredString(method, ConsoleColor.Red);
            default:
                return Logger.BuildColoredString(method, ConsoleColor.Gray);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _listener.Start();

        _ = Task.Run(async () => await HandleRequest().ConfigureAwait(false));
        _log.Info($"Http server <Green>Online</Green>! listening on port {_port}", Severity.Rest);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _listener.Stop();

        return Task.CompletedTask;
    }
}
