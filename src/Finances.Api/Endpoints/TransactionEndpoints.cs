using Finances.Api.Filters;
using Finances.Core.Contexts.TransactionContext.UseCases.Create;

namespace Finances.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTansactionEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/v1/transactions")
            .RequireAuthorization();

        group.MapPost("/", async ([FromBody] CreateTransactionRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        }).AddEndpointFilter<ValidationFilter<CreateTransactionRequest>>();
    }
}

