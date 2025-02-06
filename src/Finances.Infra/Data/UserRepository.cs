using Finances.Core.Contexts.UserContext.Data;
using Finances.Core.Contexts.UserContext.Entities;

namespace Finances.Infra.Data;

public class UserRepository : Repository<User, FinancesDbContext>, IUserRepository
{
    public UserRepository(FinancesDbContext context) : base(context) { }
}

