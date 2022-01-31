using DDocsBackend.Data;
using DDocsBackend.Data.Models;
using DDocsBackend.Helpers;
using DDocsBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;

namespace DDocsBackend;

public class RestModuleBase
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public HttpListenerContext Context { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public HttpServer? RestServer { get; private set; }

    public HttpListenerRequest Request
        => Context.Request;
    public HttpListenerResponse Response
        => Context.Response;

    public AuthenticationService AuthenticationService
        => RestServer!.Provider.GetRequiredService<AuthenticationService>();

    public DiscordOAuthHelper DiscordOAuthHelper
        => RestServer!.Provider.GetRequiredService<DiscordOAuthHelper>();

    public Authentication? Authentication { get; set; }

    public JsonSerializer Serializer
        => RestServer!.Provider.GetRequiredService<JsonSerializer>();

    public DataAccessLayer DataAccessLayer
        => RestServer!.Provider.GetRequiredService<DataAccessLayer>();

    public DiscordBridgeService DiscordService
        => RestServer!.Provider.GetRequiredService<DiscordBridgeService>();

    internal RestModuleInfo? ModuleInfo { get; private set; }

    internal RestModuleBase InitializeModule(HttpListenerContext context, RestModuleInfo info, HttpServer server)
    {
        this.Context = context;
        this.ModuleInfo = info;
        this.RestServer = server;
        return this;
    }

    protected TReturn? GetBody<TReturn>() where TReturn : class
    {
        if (!Request.HasEntityBody)
            return null;

        try
        {
            using(var sr = new StreamReader(Request.InputStream))
            using(var reader = new JsonTextReader(sr))
            {
                return Serializer.Deserialize<TReturn>(reader);
            }
        }
        catch(Exception x)
        {
            Logger.GetLogger<RestModuleBase>().Warn("Failed to read body: ", exception: x);
            return null;
        }
    }

    public override bool Equals(object? obj)
    {
        try
        {
            if (obj == null)
                return false;

            if (obj is RestModuleBase other)
            {
                return other.ModuleInfo?.Equals(this.ModuleInfo) ?? false;
            }
            else return base.Equals(obj);
        }
        catch
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
