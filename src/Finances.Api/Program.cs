using Finances.Api.Endpoints;
using Finances.Api.Extensions;
using Finances.Api.Middlewares;
using Finances.Core;
using Finances.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJsonSerializationOptionsDependencyInjection();
builder.Services.AddFluentValidationDependencyInjection();
builder.Services.AddSwaggerDependencyInjection();
builder.Services.AddHealthChecksDependencyInjection(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorizationDependencyInjection(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// custom
app.UseHealthChecksMiddleware();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapTansactionEndpoints();
app.MapUserEndpoints();
app.MapConsolidationEndpoints();

// custom
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
app.UseGlobalExceptionHandlerMiddleware(loggerFactory);

app.Run();
