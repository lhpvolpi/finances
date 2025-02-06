using Finances.Core.Contexts.SharedContext.Extesnsions;

namespace Finances.Core.Contexts.UserContext.UseCases.Shared;

public class UserBaseValidator<T> : AbstractValidator<T> where T : UserRequest
{
    public UserBaseValidator()
    {
        RuleFor(i => i.Email)
            .EmailValidator();

        RuleFor(i => i.Password)
            .PasswordValidator();
    }
}