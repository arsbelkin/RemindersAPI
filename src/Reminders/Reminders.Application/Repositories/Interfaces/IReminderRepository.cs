using Reminders.Domain.Models;
using Reminders.Application.TransferModels.Reminder;

namespace Reminders.Application.Repositories.Interfaces;

public interface IReminderRepository
{
    public Task CreateReminderAsync(Reminder reminder);
    public Task<List<ReminderListViewDTO>> GetRemindersByUserIdAsync(Guid userId);
    public IQueryable<Guid>  GetRemindersByUserIdQuery(Guid userId);
    public Task<ReminderInfoViewDTO?> GetReminderInfoAsync(Guid reminderId, Guid userId);
}