using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Api.Middlewares;

public static class GlobalExceptionHandlerMiddleware
{
    public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature.Error;

                var statusCode = StatusCodes.Status500InternalServerError;

                var response = GlobalExceptionHandlerResponse.CreateError(statusCode, message: exception.Message);

                // Log structured data
                logger.LogError(exception, "An exception occurred during request processing. Details: {@ExceptionDetails}",
                    new
                    {
                        Timestamp = DateTime.UtcNow,
                        StatusCode = statusCode,
                        Type = exception?.GetType().Name,
                        exception?.Message,
                        exception?.StackTrace,
                        HttpRequest = new
                        {
                            context.Request.Method,
                            Path = context.Request.Path.HasValue ? context.Request.Path.Value : null,
                            QueryString = context.Request.QueryString.ToString()
                        },
                        HttpResponse = new
                        {
                            response.StatusCode
                        }
                    });

                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            });
        });
    }
}

