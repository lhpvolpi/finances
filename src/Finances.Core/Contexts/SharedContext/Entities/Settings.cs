namespace Finances.Core.Contexts.SharedContext.Entities;

public interface ISettings
{
    AuthenticationSettings Authentication { get; set; }

    ConnectionStringsSettings ConnectionStrings { get; set; }
}

public class Settings : ISettings
{
    public AuthenticationSettings Authentication { get; set; }

    public ConnectionStringsSettings ConnectionStrings { get; set; }
}

public class AuthenticationSettings
{
    public string PasswordKey { get; set; }

    public string SecretKey { get; set; }

    public string Audience { get; set; }

    public string Issuer { get; set; }

    public int ExpiresInHours { get; set; }
}

public class ConnectionStringsSettings
{
    public string RedisConnection { get; set; }
}