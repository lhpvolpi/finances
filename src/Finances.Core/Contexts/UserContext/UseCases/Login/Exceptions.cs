namespace Finances.Core.Contexts.UserContext.UseCases.Login;

public class PasswordException : Exception
{
    private const string DefaultErrorMessage = "Wrong password";

    public PasswordException(string message = DefaultErrorMessage) : base(message) { }

    public static void ThrowIfNotEquals(string one, string two, string message = DefaultErrorMessage)
    {
        if (one != two)
            throw new PasswordException(message);
    }
}