using Humteria.Data.Models;

namespace Humteria.Data.Services;

public interface IMainInterface
{
    //Change Saving
    bool SaveChanges();
    Task<int> SaveChangesAsync();

    //User Handling
    Task<User?> GetUserByMail(string mail);
    Task<User?> RegisterNewUser(User user);

 
}
