using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.TransactionContext.Enums;

namespace Finances.Core.Contexts.TransactionContext.UseCases.Create;

public class CreateTransactionRequest : IRequest<Response>
{
    public ETransactionType Type { get; set; }

    public decimal Amount { get; set; }
}