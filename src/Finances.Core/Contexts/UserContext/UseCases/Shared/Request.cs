using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.UserContext.UseCases.Shared;

public class UserRequest : IRequest<Response>
{
    public string Email { get; set; }

    public string Password { get; set; }
}