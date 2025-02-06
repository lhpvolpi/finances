using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.SharedContext.Extesnsions;
using Finances.Core.Contexts.UserContext.Data;
using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Core.Contexts.UserContext.UseCases.Create;

public class RequestHandler : IRequestHandler<CreateUserRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISettings _settings;

    public RequestHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
       IOptions<Settings> settings)
    {
        this._userRepository = userRepository;
        this._unitOfWork = unitOfWork;
        this._settings = settings.Value;
    }

    public async Task<Response> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await this._userRepository.GetFirstAsync(i => i.Email == request.Email);
            AlreadyExistsException.ThrowIfNotNull(user, "email already exists");

            this._unitOfWork.BeginTransaction();

            user = new User(request.Email, SecurityExtensions.EncryptHmacSha256(this._settings.Authentication.PasswordKey, request.Password));
            this._userRepository.Insert(user);

            this._unitOfWork.Commit();

            return CreateUserResponse.CreateSuccess(200);
        }
        catch
        {
            this._unitOfWork.Rollback();
            throw;
        }
    }
}