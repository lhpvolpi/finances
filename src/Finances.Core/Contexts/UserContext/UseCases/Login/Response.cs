using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.UserContext.UseCases.Login;

public class LoginResponse : Response
{
    public const string DefaultMessageSuccess = "Login successfully created";

    public LoginResponse(int statusCode, bool success, object data, string message) : base(statusCode, success, data, message) { }

    public static LoginResponse CreateSuccess(int statusCode, object data = null, string message = DefaultMessageSuccess) => new(statusCode, true, data, message);
}