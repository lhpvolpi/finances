using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.SharedContext.Services;
using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Infra.Services;

public class TokenService : ITokenService
{
    private readonly ISettings _settings;

    public TokenService(IOptions<Settings> settings)
        => this._settings = settings.Value;

    public (string, DateTime) Generate(User user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._settings.Authentication.SecretKey);

            var claims = new[]
            {
                new Claim("user_id", user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = this._settings.Authentication.Audience,
                Issuer = this._settings.Authentication.Issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(this._settings.Authentication.ExpiresInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), tokenDescriptor.Expires!.Value);
        }
        catch
        {
            throw;
        }
    }
}

