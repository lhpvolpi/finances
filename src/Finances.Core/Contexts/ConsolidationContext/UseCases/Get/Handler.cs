using Finances.Core.Contexts.ConsolidationContext.Data;
using Finances.Core.Contexts.ConsolidationContext.Entities;
using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.UserContext.Services;

namespace Finances.Core.Contexts.ConsolidationContext.UseCases.Get;

public class RequestHandler : IRequestHandler<GetConsolidationRequest, Response>
{
    private readonly ICacheRepository _cacheRepository;
    private readonly IConsolidationRepository _consolidationRepository;
    private readonly IUserService _userService;

    public RequestHandler(ICacheRepository cacheRepository,
        IConsolidationRepository consolidationRepository,
        IUserService userService)
    {
        this._cacheRepository = cacheRepository;
        this._consolidationRepository = consolidationRepository;
        this._userService = userService;
    }

    public async Task<Response> Handle(GetConsolidationRequest request, CancellationToken cancellationToken)
    {
        var userId = this._userService.UserId;
        NotFoundException.ThrowIfDefault(userId, Guid.Empty, "User not found");

        var date = request.Date;
        var key = $"consolidation:{userId}@{date:yyyy-MM-dd}";

        var consolidationSummary = await this._cacheRepository.GetFirstAsync<ConsolidationSummary>(key);
        if (consolidationSummary is not null)
            return GetConsolidationResponse.CreateSuccess(200, consolidationSummary);

        var consolidation = await this._consolidationRepository.GetFirstAsync(i => i.UserId == userId && i.Date == date);
        if (consolidation is null)
            return GetConsolidationResponse.CreateSuccess(200, null);

        consolidationSummary = ToSummary(consolidation);
        await this._cacheRepository.SetAsync(key, consolidationSummary);

        return GetConsolidationResponse.CreateSuccess(200, consolidationSummary);
    }

    private static ConsolidationSummary ToSummary(Consolidation consolidation)
        => new()
        {
            Balance = consolidation.Balance,
            Date = consolidation.Date,
            TotalCredits = consolidation.TotalCredits,
            TotalDebits = consolidation.TotalDebits,
            UserId = consolidation.UserId
        };
}
