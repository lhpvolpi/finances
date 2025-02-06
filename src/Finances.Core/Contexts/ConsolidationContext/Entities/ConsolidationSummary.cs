namespace Finances.Core.Contexts.ConsolidationContext.Entities;

public class ConsolidationSummary
{
    public DateTime Date { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal Balance { get; set; }
    public Guid UserId { get; set; }
}

