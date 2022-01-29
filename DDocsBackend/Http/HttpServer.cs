using System.Diagnostics;
using System.Net;

namespace DDocsBackend;

public class HttpServer
{
    private readonly HttpListener _listener;
    private readonly HttpRestHandler _handler;
    private readonly Logger _log;

    public HttpServer(int port)
    {
        _log = Logger.GetLogger<HttpServer>();

        _log.Info("Creating HTTP Server...", Severity.Rest);
        _listener = new HttpListener();
#if DEBUG
        _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
#else
        _listener.Prefixes.Add($"http://*:{port}/");
#endif

        _handler = new(this);

        _listener.Start();

        _ = Task.Run(async () => await HandleRequest().ConfigureAwait(false));
        _log.Info($"Http server <Green>Online</Green>! listening on port {port}", Severity.Rest);
    }


    private async Task HandleRequest()
    {
        while (_listener.IsListening)
        {
            var context = await _listener.GetContextAsync().ConfigureAwait(false);
            _ = Task.Run(async () => await HandleContext(context).ConfigureAwait(false));
        }
    }

    private async Task HandleContext(HttpListenerContext context)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var code = await _handler.ProcessRestRequestAsync(context);
            sw.Stop();
            _log.Debug($"{sw.ElapsedMilliseconds}ms: {GetColorFromMethod(context.Request.HttpMethod)} => {context.Request.RawUrl} {code}", Severity.Rest);
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
}
