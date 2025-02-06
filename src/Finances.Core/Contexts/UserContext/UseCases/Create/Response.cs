using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.UserContext.UseCases.Create;

public class CreateUserResponse : Response
{
    public const string DefaultMessageSuccess = "User successfully created";

    public CreateUserResponse(int statusCode, bool success, object data, string message) : base(statusCode, success, data, message) { }

    public static CreateUserResponse CreateSuccess(int statusCode, object data = null, string message = DefaultMessageSuccess) => new(statusCode, true, data, message);
}