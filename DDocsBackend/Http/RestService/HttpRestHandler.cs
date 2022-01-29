﻿using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;

namespace DDocsBackend;

internal class HttpRestHandler
{
    private LinkedList<RestModuleBase?> CachedModules { get; } = new();
    private int cacheSize = 15;
    private List<RestModuleInfo> Modules { get; } = new();

    private HttpServer Server { get; }

    private readonly Logger _log;

    public HttpRestHandler(HttpServer server)
    {
        _log = Logger.GetLogger<HttpRestHandler>();

        _log.Info("Creating Rest handler...", Severity.Rest);
        this.Server = server;
        LoadRoutes();
        _log.Info($"Rest handler {Logger.BuildColoredString("Online", ConsoleColor.Green)}! Loaded {Modules.Count} Modules with {Modules.Select(x => x.Routes.Count).Sum()} routes!", Severity.Rest);
    }

    private void LoadRoutes()
    {
        var modules = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(RestModuleBase)) && x != typeof(RestModuleBase));

        foreach (var module in modules)
        {
            this.Modules.Add(new RestModuleInfo(module));
        }
    }

    public bool TryGetModule(HttpListenerRequest request, out RestModuleBase? Module, out RestModuleInfo? Info)
    {
        Module = null;
        Info = null;

        if ((Module = CachedModules.ToArray().FirstOrDefault(x => x != null && (x.ModuleInfo?.HasRoute(request) ?? false))) != null)
        {
            Info = Module.ModuleInfo;
            BumpOrEnqueueModule(Module);
            return true;
        }

        var modInfo = Modules.FirstOrDefault(x => x != null && x.HasRoute(request));

        if (modInfo == null)
            return false;

        Module = modInfo.GetInstance();

        if (Module == null)
        {
            _log.Warn($"Got null instance: {modInfo}", Severity.Rest);
            return false;
        }
        Info = modInfo;
        BumpOrEnqueueModule(Module);
        return true;
    }

    private void BumpOrEnqueueModule(RestModuleBase baseModule)
    {
        if (baseModule == null)
        {
            _log.Critical("Module null", Severity.Rest);
            return;
        }

        if (CachedModules.ToArray().Any(x => x != null && x.Equals(baseModule)))
        {
            CachedModules.Remove(baseModule);
        }
        CachedModules.AddFirst(baseModule);

        if (CachedModules.Count > cacheSize)
            CachedModules.RemoveLast();

        if (CachedModules.Any(x => x == null))
        {
            _log.Warn($"{Logger.BuildColoredString("Found null in cache, removing", ConsoleColor.Red)}", Severity.Rest);

            CachedModules.Remove((RestModuleBase?)null);
        }
    }

    public async Task<int> ProcessRestRequestAsync(HttpListenerContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "*");

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

        var moduleBase = module.InitializeModule(context, info, Server);

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
                CachedModules.Remove(moduleBase);
                return -1;
            }

            if (!result.TakeAction)
            {
                return 200;
            }

            if (result.Data != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = Encoding.UTF8;

                string json = JsonConvert.SerializeObject(result.Data);

                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(json));
            }

            context.Response.StatusCode = result.Code;
            context.Response.Close();
            return result.Code;
        }
    }
}
