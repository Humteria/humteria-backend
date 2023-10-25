using Humteria.Data.Context;
using Humteria.Data.Models;
using Humteria.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace CAS.Services.Database;

public class MainSQLServices : IMainInterface
{
    private readonly ApplicationDbContext _context;

    public MainSQLServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByMail(string mail)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == mail);
    }

    public async Task<User?> RegisterNewUser(User user)
    {
        await _context.Users.AddAsync(user);
        await SaveChangesAsync();
        return await GetUserByMail(user.Email);
    }
}