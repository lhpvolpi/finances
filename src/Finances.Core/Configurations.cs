using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core;

public static class Configurations
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        // settings
        services.Configure<Settings>(configuration);

        // mediator
        services.AddMediatR(typeof(Configurations).Assembly);
    }
}
