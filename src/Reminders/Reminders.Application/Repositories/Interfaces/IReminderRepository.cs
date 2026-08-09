using Reminders.Domain.Models;

namespace Reminders.Application.Repositories.Interfaces;

public interface IReminderRepository
{
    public Task CreateReminderAsync(Reminder reminder);
}