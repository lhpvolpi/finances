namespace Finances.Core.Contexts.SharedContext.Extesnsions;

public static class SecurityExtensions
{

    public static string EncryptHmacSha256(string secret, string value)
    {
        using var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));

        var bytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(value));
        var builder = new StringBuilder();

        for (int i = 0; i < bytes.Length; i++)
            builder.Append(bytes[i].ToString("x2"));

        return builder.ToString();
    }
}

