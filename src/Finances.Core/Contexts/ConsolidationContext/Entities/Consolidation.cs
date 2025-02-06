using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Core.Contexts.ConsolidationContext.Entities;

[Table("consolidations")]
public class Consolidation : Entity
{
    public Consolidation() { }

    public Consolidation(DateTime date,
        decimal credit,
        decimal debit,
        Guid userId)
    {
        this.Date = date;
        this.TotalCredits = credit;
        this.TotalDebits = debit;
        this.Balance = this.TotalCredits - this.TotalDebits;
        this.UserId = userId;
    }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("total_credits")]
    public decimal TotalCredits { get; set; }

    [Column("total_debits")]
    public decimal TotalDebits { get; set; }

    [Column("balance")]
    public decimal Balance { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    public User User { get; set; }

    public void Update(decimal credit,
        decimal debit)
    {
        this.TotalCredits += credit;
        this.TotalDebits += debit;
        this.Balance = this.TotalCredits - this.TotalDebits;
        this.UpdatedAt = DateTime.UtcNow;
    }
}

