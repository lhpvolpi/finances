using Finances.Core.Contexts.TransactionContext.Entities;

namespace Finances.Core.Contexts.ConsolidationContext.Services;

public interface IConsolidationService
{
    Task ConsolidateAsync(TransactionCreatedEvent transactionCreatedEvent);
}

