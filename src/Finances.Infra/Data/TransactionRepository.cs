using Finances.Core.Contexts.TransactionContext.Data;
using Finances.Core.Contexts.TransactionContext.Entities;

namespace Finances.Infra.Data;

public class TransactionRepository : Repository<Transaction, FinancesDbContext>, ITransactionRepository
{
    public TransactionRepository(FinancesDbContext context) : base(context) { }
}

