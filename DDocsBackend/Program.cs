using DDocsBackend;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DDocsBackend.Data;

Logger.AddStream(Console.OpenStandardOutput(), StreamType.StandardOut);
Logger.AddStream(Console.OpenStandardError(), StreamType.StandardError);

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
        .AddSingleton<DataAccessLayer>()
        .AddSingleton(new HttpServer(4048));
    });


await Task.Delay(-1);