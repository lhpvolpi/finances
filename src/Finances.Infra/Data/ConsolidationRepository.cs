using Finances.Core.Contexts.ConsolidationContext.Data;
using Finances.Core.Contexts.ConsolidationContext.Entities;

namespace Finances.Infra.Data;

public class ConsolidationRepository : Repository<Consolidation, FinancesDbContext>, IConsolidationRepository
{
    public ConsolidationRepository(FinancesDbContext context) : base(context) { }
}

