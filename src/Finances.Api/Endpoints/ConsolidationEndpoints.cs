using Finances.Api.Filters;
using Finances.Core.Contexts.ConsolidationContext.UseCases.Get;

namespace Finances.Api.Endpoints;

public static class ConsolidationEndpoints
{
    public static void MapConsolidationEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/v1/consolidations")
            .RequireAuthorization();

        group.MapGet("/", async ([AsParameters] GetConsolidationRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        }).AddEndpointFilter<ValidationFilter<GetConsolidationRequest>>();
    }
}

