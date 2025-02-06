using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.TransactionContext.UseCases.Create;

public class CreateTransactionResponse : Response
{
    public const string DefaultMessageSuccess = "Transaction successfully created";

    public CreateTransactionResponse(int statusCode, bool success, object data, string message) : base(statusCode, success, data, message) { }

    public static CreateTransactionResponse CreateSuccess(int statusCode, object data = null, string message = DefaultMessageSuccess) => new(statusCode, true, data, message);
}