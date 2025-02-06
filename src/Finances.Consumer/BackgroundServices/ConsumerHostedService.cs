using Finances.Core.Contexts.ConsolidationContext.Services;
using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.TransactionContext.Entities;

namespace Finances.Consumer.BackgroundServices;

public class ConsumerHostedService : IHostedService, IDisposable
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _serviceScope;
    private readonly ICacheRepository _cacheRepository;
    private readonly IConsolidationService _consolidationService;
    private bool Stop { get; set; } = false;

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
        this._cacheRepository = this._serviceProvider.GetService<ICacheRepository>();
        this._serviceScope = this._serviceProvider.CreateScope();
        this._consolidationService = this._serviceScope.ServiceProvider.GetService<IConsolidationService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var queue = "finances-consolidation-queue";
        this._logger.LogInformation("[information] ConsumerHostedService started. Listening to queue: {Queue}", queue);

        while (!cancellationToken.IsCancellationRequested && !this.Stop)
        {
            try
            {
                this._logger.LogDebug("[debug] Waiting for messages in queue: {Queue}", queue);

                TransactionCreatedEvent transactionCreatedEvent = await this._cacheRepository.PopAsync<TransactionCreatedEvent>(queue);

                if (transactionCreatedEvent == default)
                {
                    this._logger.LogDebug("[debug] No message found in queue: {Queue}. Continuing...", queue);
                    continue;
                }

                this._logger.LogInformation(
                    "[information] Processing transaction {TransactionId} for user {UserId} with amount {Amount}",
                    transactionCreatedEvent.TransactionId, transactionCreatedEvent.UserId, transactionCreatedEvent.Amount
                );

                await this._consolidationService.ConsolidateAsync(transactionCreatedEvent);

                this._logger.LogInformation(
                    "[information] Transaction {TransactionId} successfully consolidated",
                    transactionCreatedEvent.TransactionId
                );
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "[error] Failed to process message from queue {Queue}", queue);
            }
        }

        this._logger.LogInformation("[information] ConsumerHostedService main loop has ended.");
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        this.Stop = true;
        this._logger.LogInformation("[information] ConsumerHostedService is stopping...");
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        this._logger.LogInformation("[information] ConsumerHostedService is being disposed.");
        GC.SuppressFinalize(this);
    }
}

