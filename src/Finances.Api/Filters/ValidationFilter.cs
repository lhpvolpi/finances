using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
        => this._validator = validator;

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument is null)
            return Results.BadRequest(new { Errors = new[] { "Invalid request body." } });

        var validationResult = await this._validator.ValidateAsync(argument);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });

            return Results.BadRequest(ValidationErrorResponse.CreateError(StatusCodes.Status400BadRequest, data: errors));
        }

        return await next(context);
    }
}

