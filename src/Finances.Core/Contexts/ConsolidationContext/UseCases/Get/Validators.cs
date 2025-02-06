namespace Finances.Core.Contexts.ConsolidationContext.UseCases.Get;

public class GetConsolidationValidator : AbstractValidator<GetConsolidationRequest>
{
    public GetConsolidationValidator()
    {
        RuleFor(i => i.Date)
            .NotEmpty()
                .WithMessage("date is required");
    }
}
