using Finances.Core.Contexts.ConsolidationContext.Data;
using Finances.Core.Contexts.ConsolidationContext.Entities;
using Finances.Core.Contexts.MessageContext.Data;
using Finances.Core.Contexts.MessageContext.Entities;
using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.TransactionContext.Entities;
using Finances.Core.Contexts.TransactionContext.Enums;
using Finances.Infra.Services;

namespace Finances.Tests.ServiceTests;

public class ConsolidationServiceTest
{
    private readonly Mock<IConsolidationRepository> _consolidationRepositoryMock;
    private readonly Mock<IMessageRepository> _messageRepositoryMock;
    private readonly Mock<ICacheRepository> _cacheRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ConsolidationService _consolidationService;

    public ConsolidationServiceTest()
    {
        this._consolidationRepositoryMock = new Mock<IConsolidationRepository>();
        this._messageRepositoryMock = new Mock<IMessageRepository>();
        this._cacheRepositoryMock = new Mock<ICacheRepository>();
        this._unitOfWorkMock = new Mock<IUnitOfWork>();
        this._consolidationService = new ConsolidationService(
            this._consolidationRepositoryMock.Object,
            this._messageRepositoryMock.Object,
            this._cacheRepositoryMock.Object,
            this._unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ConsolidateAsync_SuccessfulConsolidation()
    {
        // Arrange
        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            OccurredAt = DateTime.UtcNow,
            TransactionType = ETransactionType.CREDIT,
            Amount = 100,
            UserId = Guid.NewGuid(),
            MessageId = Guid.NewGuid()
        };

        var message = new Message
        {
            Id = transactionCreatedEvent.MessageId.Value,
            Processed = false
        };

        this._messageRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Message, bool>>>())).ReturnsAsync(message);
        this._consolidationRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Consolidation, bool>>>())).ReturnsAsync((Consolidation)null);
        this._cacheRepositoryMock.Setup(i => i.HasKeyAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        await this._consolidationService.ConsolidateAsync(transactionCreatedEvent);

        // Assert
        this._unitOfWorkMock.Verify(i => i.BeginTransaction(), Times.Once);
        this._consolidationRepositoryMock.Verify(i => i.Insert(It.IsAny<Consolidation>()), Times.Once);
        this._messageRepositoryMock.Verify(i => i.Update(It.IsAny<Message>()), Times.Once);
        this._cacheRepositoryMock.Verify(i => i.SetAsync(It.IsAny<string>(), It.IsAny<ConsolidationSummary>(), 600), Times.Once);
        this._unitOfWorkMock.Verify(i => i.Commit(), Times.Once);
    }

    [Fact]
    public async Task ConsolidateAsync_MessageNotFound()
    {
        // Arrange
        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            OccurredAt = DateTime.UtcNow,
            TransactionType = ETransactionType.CREDIT,
            Amount = 100,
            UserId = Guid.NewGuid(),
            MessageId = Guid.NewGuid()
        };

        this._messageRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Message, bool>>>())).ReturnsAsync((Message)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => this._consolidationService.ConsolidateAsync(transactionCreatedEvent));
        this._unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task ConsolidateAsync_MessageAlreadyProcessed()
    {
        // Arrange
        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            OccurredAt = DateTime.UtcNow,
            TransactionType = ETransactionType.CREDIT,
            Amount = 100,
            UserId = Guid.NewGuid(),
            MessageId = Guid.NewGuid()
        };

        var message = new Message
        {
            Id = transactionCreatedEvent.MessageId.Value,
            Processed = true
        };

        this._messageRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Message, bool>>>())).ReturnsAsync(message);

        // Act
        await _consolidationService.ConsolidateAsync(transactionCreatedEvent);

        // Assert
        this._unitOfWorkMock.Verify(i => i.BeginTransaction(), Times.Once);
        this._unitOfWorkMock.Verify(i => i.Commit(), Times.Never);
    }

    [Fact]
    public async Task ConsolidateAsync_ConsolidationAlreadyExists()
    {
        // Arrange
        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            OccurredAt = DateTime.UtcNow,
            TransactionType = ETransactionType.CREDIT,
            Amount = 100,
            UserId = Guid.NewGuid(),
            MessageId = Guid.NewGuid()
        };

        var message = new Message
        {
            Id = transactionCreatedEvent.MessageId.Value,
            Processed = false
        };

        var consolidation = new Consolidation(transactionCreatedEvent.OccurredAt.Date, 0, 0, transactionCreatedEvent.UserId);

        this._messageRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Message, bool>>>())).ReturnsAsync(message);
        this._consolidationRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Consolidation, bool>>>())).ReturnsAsync(consolidation);
        this._cacheRepositoryMock.Setup(i => i.HasKeyAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        await this._consolidationService.ConsolidateAsync(transactionCreatedEvent);

        // Assert
        this._unitOfWorkMock.Verify(i => i.BeginTransaction(), Times.Once);
        this._consolidationRepositoryMock.Verify(i => i.Update(It.IsAny<Consolidation>()), Times.Once);
        this._messageRepositoryMock.Verify(i => i.Update(It.IsAny<Message>()), Times.Once);
        this._cacheRepositoryMock.Verify(i => i.SetAsync(It.IsAny<string>(), It.IsAny<ConsolidationSummary>(), 600), Times.Once);
        this._unitOfWorkMock.Verify(i => i.Commit(), Times.Once);
    }

    [Fact]
    public async Task ConsolidateAsync_CacheKeyExists()
    {
        // Arrange
        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            OccurredAt = DateTime.UtcNow,
            TransactionType = ETransactionType.CREDIT,
            Amount = 100,
            UserId = Guid.NewGuid(),
            MessageId = Guid.NewGuid()
        };

        var message = new Message
        {
            Id = transactionCreatedEvent.MessageId.Value,
            Processed = false
        };

        this._messageRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Message, bool>>>())).ReturnsAsync(message);
        this._consolidationRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Consolidation, bool>>>())).ReturnsAsync((Consolidation)null);
        this._cacheRepositoryMock.Setup(i => i.HasKeyAsync(It.IsAny<string>())).ReturnsAsync(true);

        // Act
        await this._consolidationService.ConsolidateAsync(transactionCreatedEvent);

        // Assert
        this._unitOfWorkMock.Verify(i => i.BeginTransaction(), Times.Once);
        this._cacheRepositoryMock.Verify(i => i.RemoveAsync(It.IsAny<string>()), Times.Once);
        this._cacheRepositoryMock.Verify(i => i.SetAsync(It.IsAny<string>(), It.IsAny<ConsolidationSummary>(), 600), Times.Once);
        this._unitOfWorkMock.Verify(i => i.Commit(), Times.Once);
    }

    [Fact]
    public async Task ConsolidateAsync_ExceptionHandling()
    {
        // Arrange
        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            OccurredAt = DateTime.UtcNow,
            TransactionType = ETransactionType.CREDIT,
            Amount = 100,
            UserId = Guid.NewGuid(),
            MessageId = Guid.NewGuid()
        };

        var message = new Message
        {
            Id = transactionCreatedEvent.MessageId.Value,
            Processed = false
        };

        this._messageRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Message, bool>>>())).ReturnsAsync(message);
        this._consolidationRepositoryMock.Setup(i => i.GetFirstAsync(It.IsAny<Expression<Func<Consolidation, bool>>>())).ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => this._consolidationService.ConsolidateAsync(transactionCreatedEvent));
        this._unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
    }
}