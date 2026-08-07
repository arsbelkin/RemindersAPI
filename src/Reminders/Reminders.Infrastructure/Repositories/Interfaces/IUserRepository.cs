using Reminders.Domain.Models;

namespace Reminders.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    public Task CreateUserAsync(User user);
}