using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.TransactionContext.Enums;
using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Core.Contexts.TransactionContext.Entities;

[Table("transactions")]
public class Transaction : Entity
{
    public Transaction() { }

    public Transaction(ETransactionType type, decimal amount, Guid userId)
    {
        this.Type = type;
        this.Amount = amount;
        this.UserId = userId;
    }

    [Column("type")]
    public ETransactionType Type { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("occurred_at")]
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    [Column("user_id")]
    public Guid UserId { get; set; }

    public User User { get; set; }
}

