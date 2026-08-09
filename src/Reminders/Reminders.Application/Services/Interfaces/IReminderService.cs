using Reminders.Application.TransferModels.Reminder;

namespace Reminders.Application.Services.Interfaces;

public interface IReminderService
{
    public Task<Guid> CreateReminderAsync(ReminderCreateDTO dto);
    public Task<List<ReminderViewDTO>> GetUserRemindersAsync(Guid userId);
}