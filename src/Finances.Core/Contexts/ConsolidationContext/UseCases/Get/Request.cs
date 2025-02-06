using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.ConsolidationContext.UseCases.Get;

public class GetConsolidationRequest : IRequest<Response>
{
    public DateTime Date { get; set; }
}