using Finances.Api.Filters;
using Finances.Core.Contexts.UserContext.UseCases.Create;
using Finances.Core.Contexts.UserContext.UseCases.Login;

namespace Finances.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/v1/users");

        group.MapPost("/", async ([FromBody] CreateUserRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        }).AddEndpointFilter<ValidationFilter<CreateUserRequest>>();

        group.MapPost("/login", async ([FromBody] LoginRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        }).AddEndpointFilter<ValidationFilter<LoginRequest>>();
    }
}

