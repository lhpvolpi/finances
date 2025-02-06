using Finances.Core.Contexts.TransactionContext.Enums;

namespace Finances.Core.Contexts.TransactionContext.Entities;

public class TransactionCreatedEvent
{
    public Guid? MessageId { get; set; }
    public ETransactionEventType EventType { get; set; }
    public Guid TransactionId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public ETransactionType TransactionType { get; set; }
    public DateTime OccurredAt { get; set; }
}