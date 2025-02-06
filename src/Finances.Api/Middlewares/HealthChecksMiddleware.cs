namespace Finances.Api.Middlewares;

public static class HealthChecksMiddleware
{
    public static void UseHealthChecksMiddleware(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health-details", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    monitors = report.Entries.Select(i => new
                    {
                        key = i.Key,
                        value = Enum.GetName(typeof(HealthStatus), i.Value.Status)
                    })
                });

                context.Response.ContentType = MediaTypeNames.Application.Json;

                await context.Response.WriteAsync(result);
            }
        });
    }
}

