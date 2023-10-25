using Humteria.Data.Models;

namespace Humteria.Data.Services;

public interface IMainInterface
{
    // Save Changes
    bool SaveChanges();
    Task<int> SaveChangesAsync();

    // User
    Task<User?> GetUserByMail(string mail);
    Task<User?> RegisterNewUser(User user);
}
