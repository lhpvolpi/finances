namespace Finances.Core.Contexts.TransactionContext.UseCases.Create;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionValidator()
    {
        RuleFor(i => i.Type)
            .NotEmpty()
                .WithMessage("transaction type is required");

        RuleFor(i => i.Amount)
            .NotEmpty()
                .WithMessage("amount is required")
            .GreaterThan(0)
                .WithMessage("amount must be greater than zero");
    }
}