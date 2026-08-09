using Reminders.Application.Repositories.Interfaces;
using Reminders.Domain.Models;
using Reminders.Infrastructure.Contexts;

namespace Reminders.Infrastructure.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly RemindersContext _dbContext;
    
    public ReminderRepository(RemindersContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateReminderAsync(Reminder reminder)
    {
        _dbContext.Reminders.Add(reminder);
        await _dbContext.SaveChangesAsync();
    }
}