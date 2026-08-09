using Microsoft.EntityFrameworkCore;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.TransferModels.Reminder;
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

    public async Task<List<ReminderViewDTO>> GetRemindersByUserIdAsync(Guid userId)
    {
        var reminders = await _dbContext.Reminders
            .Where(r => r.Members.Any(m => m.Id == userId))
            .Select(r => new ReminderViewDTO
            {
                Id = r.Id,
                CategoryId = r.CategoryId,
                CategoryName = r.Category.Title,
                Title = r.Title,
                Description = r.Description,
                CreatedTime = r.CreatedTime,
                Priority = r.Priority,
                IsCompleted = r.IsCompleted,
                CompletedTime = r.CompletedTime,
                DueDate = r.DueDate,
                MemberEmails = r.Members.Select( m => m.Email).ToList()
            })
            .ToListAsync();

        return reminders;
    }
}