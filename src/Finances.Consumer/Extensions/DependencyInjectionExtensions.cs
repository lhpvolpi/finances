using Finances.Consumer.BackgroundServices;

namespace Finances.Consumer.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // consumer
        services.AddSingleton<ConsumerHostedService>();
        services.AddHostedService<FinancesHostedService<ConsumerHostedService>>();

        services.AddHttpContextAccessor();
    }
}

