using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;

namespace DDocsBackend;

internal class HttpRestHandler
{
    public const int CacheSize = 32;

    private readonly List<RestModuleInfo> _modules = new();

    private readonly HttpServer _server;
    private readonly Logger _log;
    private readonly JsonSerializer _serializer;
    private readonly MemoryCache _cache;

    public HttpRestHandler(HttpServer server)
    {
        _serializer = server.Provider.GetRequiredService<JsonSerializer>();
        _log = Logger.GetLogger<HttpRestHandler>();

        _log.Info("Creating Rest handler...", Severity.Rest);
        this._server = server;
        LoadRoutes();
        _log.Info($"Rest handler {Logger.BuildColoredString("Online", ConsoleColor.Green)}! Loaded {_modules.Count} Modules with {_modules.Select(x => x.Routes.Count).Sum()} routes!", Severity.Rest);
        _cache = new MemoryCache("modules");
    }

    private void LoadRoutes()
    {
        var modules = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(RestModuleBase)) && x != typeof(RestModuleBase));

        foreach (var module in modules)
        {
            this._modules.Add(new RestModuleInfo(module, _server.Provider));
        }
    }

    public bool TryGetModule(HttpListenerRequest request, out RestModuleBase? Module, out RestModuleInfo? Info)
    {
        Module = null;
        Info = null;

        var modInfo = _modules.FirstOrDefault(x => x != null && x.HasRoute(request));

        if (modInfo == null)
            return false;

        Info = modInfo;

        var route = modInfo.GetRoute(request);

        Module = (RestModuleBase?)_cache.Get($"{route!.RouteMethod}{route.RouteName}");

        if (Module != null)
            return true;

        Module = modInfo.GetInstance();

        if (Module == null)
        {
            _log.Warn($"Got null instance: {modInfo}", Severity.Rest);
            return false;
        }
        return true;
    }

    private void CacheModule(RestModuleBase module, RestMethodInfo route)
    {
        _cache.Set($"{route.RouteMethod}{route.RouteName}", module, DateTimeOffset.UtcNow.AddHours(1));

    }

    public async Task<int> ProcessRestRequestAsync(HttpListenerContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", $"{context.Request.Headers["origin"]}");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

        if(context.Request.HttpMethod == "OPTIONS")
        {
            context.Response.Headers.Add("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"] ?? "*");
            context.Response.StatusCode = 200;
            context.Response.Close();
            return 200;
        }

        context.Response.Headers.Add("Access-Control-Allow-Headers", "*");

        if (!TryGetModule(context.Request, out var module, out var info))
        {
            context.Response.StatusCode = 404;
            context.Response.Close();
            return 404;
        }

        if (module == null || info == null)
        {
            context.Response.StatusCode = 404;
            context.Response.Close();
            return 404;
        }

        var moduleBase = module.InitializeModule(context, info, _server);

        var route = info.GetRoute(context.Request);

        if (route == null)
        {
            context.Response.StatusCode = 404;
            context.Response.Close();
            return 404;
        }

        var parms = route.GetConvertedParameters(context.Request.RawUrl!);

        if (parms == null)
        {
            context.Response.StatusCode = 400;
            context.Response.Close();
            return 400;
        }

        var task = route.ExecuteAsync(moduleBase, parms);
        var result = await task;

        if (task.Exception != null)
        {
            _log.Error($"Uncaught exception in route {route.RouteName}!\n{task.Exception}", Severity.Rest);
            context.Response.StatusCode = 500;
            context.Response.Close();
            return 500;
        }
        else
        {
            if (result.Code == int.MaxValue)
            {
                return -1;
            }

            if (!result.TakeAction)
            {
                return 200;
            }

            context.Response.StatusCode = result.Code;

            if (result.Data != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = Encoding.UTF8;

                using (var sw = new StreamWriter(context.Response.OutputStream)) 
                using(var writer = new JsonTextWriter(sw))
                {
                    _serializer.Serialize(writer, result.Data);
                }    
            }

            context.Response.Close();
            CacheModule(module, route);
            return result.Code;
        }
    }
}
