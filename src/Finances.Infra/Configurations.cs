using Finances.Core.Contexts.ConsolidationContext.Data;
using Finances.Core.Contexts.ConsolidationContext.Services;
using Finances.Core.Contexts.MessageContext.Data;
using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Services;
using Finances.Core.Contexts.TransactionContext.Data;
using Finances.Core.Contexts.UserContext.Data;
using Finances.Core.Contexts.UserContext.Services;
using Finances.Infra.Data;
using Finances.Infra.Services;

namespace Finances.Infra;

public static class Configurations
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // context
        services.AddDbContext<FinancesDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // data
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // cache
        services.AddTransient<ICacheRepository, CacheRepository>();

        // repositories
        services.AddTransient<IConsolidationRepository, ConsolidationRepository>();
        services.AddTransient<IMessageRepository, MessageRepository>();
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        // services
        services.AddTransient<IConsolidationService, ConsolidationService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
    }
}

