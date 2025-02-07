using Finances.Core.Contexts.ConsolidationContext.Data;
using Finances.Core.Contexts.ConsolidationContext.Entities;
using Finances.Core.Contexts.ConsolidationContext.Services;
using Finances.Core.Contexts.MessageContext.Data;
using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.TransactionContext.Entities;
using Finances.Core.Contexts.TransactionContext.Enums;

namespace Finances.Infra.Services;

public class ConsolidationService : IConsolidationService
{
    private readonly IConsolidationRepository _consolidationRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly ICacheRepository _cacheRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConsolidationService(IConsolidationRepository consolidationRepository,
        IMessageRepository messageRepository,
        ICacheRepository cacheRepository,
        IUnitOfWork unitOfWork)
    {
        this._consolidationRepository = consolidationRepository;
        this._messageRepository = messageRepository;
        this._cacheRepository = cacheRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task ConsolidateAsync(TransactionCreatedEvent transactionCreatedEvent)
    {
        try
        {
            this._unitOfWork.BeginTransaction();

            var date = transactionCreatedEvent.OccurredAt.Date;
            var credit = transactionCreatedEvent.TransactionType == ETransactionType.CREDIT ? Math.Max(transactionCreatedEvent.Amount, 0) : 0;
            var debit = transactionCreatedEvent.TransactionType == ETransactionType.DEBIT ? Math.Max(transactionCreatedEvent.Amount, 0) : 0;
            var userId = transactionCreatedEvent.UserId;
            var key = $"consolidation:{userId}@{date:yyyy-MM-dd}";

            var message = await this._messageRepository.GetFirstAsync(i => i.Id == transactionCreatedEvent.MessageId);
            NotFoundException.ThrowIfNull(message, "message not found");

            if (message.Processed)
                return;

            var consolidation = await this._consolidationRepository.GetFirstAsync(i => i.UserId == userId && i.Date == date);

            if (consolidation is null)
            {
                consolidation = new Consolidation(date, credit, debit, userId);
                this._consolidationRepository.Insert(consolidation);
            }
            else
            {
                consolidation.Update(credit, debit);
                this._consolidationRepository.Update(consolidation);
            }

            message.MarkAsProcessed();
            this._messageRepository.Update(message);

            if (await this._cacheRepository.HasKeyAsync(key))
                await this._cacheRepository.RemoveAsync(key);

            await this._cacheRepository.SetAsync(key, ToSummary(consolidation));

            this._unitOfWork.Commit();
        }
        catch
        {
            this._unitOfWork.Rollback();
            throw;
        }
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