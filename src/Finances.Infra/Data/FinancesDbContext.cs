using Finances.Core.Contexts.ConsolidationContext.Entities;
using Finances.Core.Contexts.MessageContext.Entities;
using Finances.Core.Contexts.TransactionContext.Entities;
using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Infra.Data;

public class FinancesDbContext : DbContext
{
    public FinancesDbContext(DbContextOptions<FinancesDbContext> options) : base(options) { }

    public DbSet<Consolidation> Consolidations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
}

