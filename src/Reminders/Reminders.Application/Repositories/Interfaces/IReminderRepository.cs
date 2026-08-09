using Reminders.Domain.Models;
using Reminders.Application.TransferModels.Reminder;

namespace Reminders.Application.Repositories.Interfaces;

public interface IReminderRepository
{
    public Task CreateReminderAsync(Reminder reminder);
    public Task<List<ReminderViewDTO>> GetRemindersByUserIdAsync(Guid userId);
}