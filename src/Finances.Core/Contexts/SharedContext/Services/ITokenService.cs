using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Core.Contexts.SharedContext.Services;

public interface ITokenService
{
    (string, DateTime) Generate(User user);
}

