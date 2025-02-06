using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.UserContext.Entities;

[Table("users")]
public class User : Entity
{
    public User(string email, string passwordHash)
    {
        this.Email = email;
        this.PasswordHash = passwordHash;
    }

    [Column("email")]
    public string Email { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; }
}

