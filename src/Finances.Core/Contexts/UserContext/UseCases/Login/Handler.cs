using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.SharedContext.Extesnsions;
using Finances.Core.Contexts.SharedContext.Services;
using Finances.Core.Contexts.UserContext.Data;

namespace Finances.Core.Contexts.UserContext.UseCases.Login;

public class RequestHandler : IRequestHandler<LoginRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ISettings _settings;

    public RequestHandler(IUserRepository userRepository,
        ITokenService tokenService,
        IOptions<Settings> settings)
    {
        this._userRepository = userRepository;
        this._tokenService = tokenService;
        this._settings = settings.Value;
    }

    public async Task<Response> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var defaultMessage = "Invalid credentials";

            var user = await this._userRepository.GetFirstAsync(i => i.Email == request.Email);
            NotFoundException.ThrowIfNull(user, defaultMessage);

            var passwordHashRequest = SecurityExtensions.EncryptHmacSha256(this._settings.Authentication.PasswordKey, request.Password);
            PasswordException.ThrowIfNotEquals(passwordHashRequest, user.PasswordHash, defaultMessage);

            var token = this._tokenService.Generate(user);

            return LoginResponse.CreateSuccess(200, new
            {
                Token = token.Item1,
                ExpiresIn = token.Item2
            });
        }
        catch
        {
            throw;
        }
    }
}