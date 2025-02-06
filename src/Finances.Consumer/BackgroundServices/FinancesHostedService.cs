using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Finances.Consumer.BackgroundServices;

public class FinancesHostedService<T> : IHostedService where T : IHostedService
{
    private readonly T _service;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _serviceScope;

    public FinancesHostedService(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
        this._serviceScope = this._serviceProvider.CreateScope();
        this._service = this._serviceScope.ServiceProvider.GetService<T>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this._service.StartAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this._service.StopAsync(cancellationToken);
        return Task.CompletedTask;
    }
}

