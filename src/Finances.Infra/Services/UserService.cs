using Finances.Core.Contexts.UserContext.Services;

namespace Finances.Infra.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
        this.UserId = Convert<Guid>(this.GetClaim("user_id")?.Value);
    }

    public Guid UserId { get; set; }

    private Claim GetClaim(string claimType)
        => this._httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(i => i.Type == claimType);

    private static T Convert<T>(string input)
    {
        try
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(input);
        }
        catch
        {
            return default;
        }
    }
}

