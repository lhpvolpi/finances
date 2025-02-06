using Finances.Core.Contexts.MessageContext.Data;
using Finances.Core.Contexts.MessageContext.Entities;
using Finances.Core.Contexts.MessageContext.Enums;
using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.TransactionContext.Data;
using Finances.Core.Contexts.TransactionContext.Entities;
using Finances.Core.Contexts.UserContext.Services;

namespace Finances.Core.Contexts.TransactionContext.UseCases.Create;

public class RequestHandler : IRequestHandler<CreateTransactionRequest, Response>
{
    private readonly IUserService _userService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly ICacheRepository _cacheRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestHandler(IUserService userService,
        ITransactionRepository transactionRepository,
        IMessageRepository messageRepository,
        ICacheRepository cacheRepository,
        IUnitOfWork unitOfWork)
    {
        this._userService = userService;
        this._transactionRepository = transactionRepository;
        this._messageRepository = messageRepository;
        this._cacheRepository = cacheRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = this._userService.UserId;
            NotFoundException.ThrowIfDefault(userId, Guid.Empty, "user not found");

            this._unitOfWork.BeginTransaction();

            var transaction = new Transaction(request.Type, request.Amount, userId);
            this._transactionRepository.Insert(transaction);

            var transactionCreatedEvent = CreatedEvent(transaction);

            var message = new Message(EMessageType.TRANSACTION_CREATED, JsonSerializer.Serialize(transactionCreatedEvent, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }));

            this._messageRepository.Insert(message);

            transactionCreatedEvent.MessageId = message.Id;

            await this._cacheRepository.PushAsync("finances-consolidation-queue", transactionCreatedEvent);

            this._unitOfWork.Commit();

            return CreateTransactionResponse.CreateSuccess(200, transaction);
        }
        catch
        {
            this._unitOfWork.Rollback();
            throw;
        }
    }

    public static TransactionCreatedEvent CreatedEvent(Transaction transaction)
        => new()
        {
            Amount = transaction.Amount,
            EventType = Enums.ETransactionEventType.TRANSACTION_CREATED,
            OccurredAt = transaction.OccurredAt,
            TransactionId = transaction.Id,
            TransactionType = transaction.Type,
            UserId = transaction.UserId
        };
}