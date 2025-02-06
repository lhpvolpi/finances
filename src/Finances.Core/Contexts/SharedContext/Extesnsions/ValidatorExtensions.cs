namespace Finances.Core.Contexts.SharedContext.Extesnsions;

public static class ValidatorExtensions
{
    public static Dictionary<string, string[]> ValidationResultErrorsValidator(this List<ValidationFailure> validationFailures)
    => validationFailures.GroupBy(i => i.PropertyName, i => i.ErrorMessage, (propertyName, errorMessages) => new
    {
        Key = propertyName,
        Values = errorMessages.Distinct().ToArray()
    }).ToDictionary(i => i.Key, i => i.Values);

    public static IRuleBuilder<T, string> PasswordValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder
        .NotEmpty()
            .WithMessage("password is required")
        .MinimumLength(8)
            .WithMessage("password must contain at least 8 characters")
        .Matches("[a-z]+")
            .WithMessage("password must contain lowercase letters")
        .Matches("[A-Z]+")
            .WithMessage("password must contain uppercase letters")
        .Matches("(\\d)+")
            .WithMessage("password must contain numbers")
        .Matches("(\\W)+")
            .WithMessage("password contain symbols");

    public static IRuleBuilder<T, string> EmailValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder
        .NotEmpty()
            .WithMessage("e-mail is required")
        .EmailAddress()
            .WithMessage("e-mail invalid");
}

