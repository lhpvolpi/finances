using Finances.Consumer.Extensions;
using Finances.Core;
using Finances.Infra;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();
    })
    .ConfigureLogging((context, builder) => builder.AddConsole())
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationServices();
        services.AddCoreServices(context.Configuration);
        services.AddInfrastructureServices(context.Configuration);
    })
    .Build();

await host.RunAsync();
