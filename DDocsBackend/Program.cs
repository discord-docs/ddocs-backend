global using DDocs;
using DDocsBackend;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DDocsBackend.Data;
using DDocsBackend.Data.Context;
using Microsoft.EntityFrameworkCore;
using DDocsBackend.Services;
using DDocsBackend.Helpers;
using Newtonsoft.Json;
using DDocsBackend.Converters;
using DDocsBackend.Data.Cached;

Logger.AddStream(Console.OpenStandardOutput(), StreamType.StandardOut);
Logger.AddStream(Console.OpenStandardError(), StreamType.StandardError);

var log = Logger.GetLogger<Program>();

try
{
    var builder = new HostBuilder()
    .ConfigureAppConfiguration(x =>
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        x.AddConfiguration(config);
    })
    .ConfigureServices((context, services) =>
    {
        // configure our services
        services
        .AddEntityFrameworkNpgsql()
        .AddDbContextFactory<DDocsContext>(x =>
            {
                x.UseModel(DDocsContextModel.Instance);
                x.UseNpgsql(context.Configuration["CONNECTION_STRING"]);
            })
        .AddSingleton(x => new JsonSerializer { ContractResolver = new DDocsContractResolver() })
        .AddSingleton<DiscordBridgeService>()
        .AddSingleton<DataAccessLayer>()
        .AddSingleton<DiscordOAuthHelper>()
        .AddSingleton<AuthenticationService>()
        .AddSingleton<CDNService>()
        .AddHostedService<HttpServer>();
    })
    .UseConsoleLifetime();

    using (var host = builder.Build())
    {
        await host.RunAsync();
    }
}
catch(Exception x)
{
    log.Critical("Failed to run main", exception: x);
}

await Task.Delay(-1);