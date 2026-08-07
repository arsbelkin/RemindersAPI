using Microsoft.EntityFrameworkCore;
using Reminders.Domain.Models;
using Reminders.Infrastructure.Repositories.Interfaces;
using Reminders.Infrastructure.Contexts;

namespace Reminders.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RemindersContext _dbContext;
    
    public UserRepository(RemindersContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckUserByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserLoginAsync(string inputString)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => (u.Email == inputString) || 
                                                        (u.Username == inputString));

        return user;
    }
}