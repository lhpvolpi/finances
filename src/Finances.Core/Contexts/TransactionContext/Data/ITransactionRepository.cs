using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.TransactionContext.Entities;

namespace Finances.Core.Contexts.TransactionContext.Data
{
    public interface ITransactionRepository : IRepository<Transaction> { }
}

