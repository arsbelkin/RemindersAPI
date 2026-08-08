using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface IUserRepository
{
    public Task CreateUserAsync(User user);
    public Task<bool> CheckUserByEmailAsync(string email);
    public Task<User?> GetUserByUsernameOrEmailAsync(string inputString);
}