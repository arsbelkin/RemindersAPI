using MapsterMapper;
using Reminders.Application.Common;
using Reminders.Application.Exceptions;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Reminder;
using Reminders.Domain.Enums;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public class ReminderService : IReminderService
{
    private readonly IReminderRepository _reminderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ReminderService(
        IReminderRepository reminderRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper
        )
    {
        _reminderRepository = reminderRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Guid> CreateReminderAsync(ReminderCreateDTO dto)
    {
        foreach (var email in dto.UsersEmails)
        {
            if (!EmailValidator.IsValid(email))
                throw new NotValidEmailException(email);
            
            if (!await _userRepository.CheckUserByEmailAsync(email))
                throw new NotValidEmailException(email);

            if (!await _categoryRepository.CheckCategoryByIdAsync(dto.CategoryId))
                throw new NotValidCategoryException();
        }
        
        var users = await _userRepository.GetAllUsersByEmailAsync(dto.UsersEmails);
        
        var reminder = new Reminder
        {
            Id = Guid.CreateVersion7(),
            CreatorId = dto.CreatorId,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedTime = DateTime.UtcNow,
            Priority = dto.Priority,
            IsCompleted = CompletedStatusTypes.NotCompleted,
            CompletedTime = null,
            DueDate = dto.DueDate,
            Members = users
        };
        
        await _reminderRepository.CreateReminderAsync(reminder);
        
        return reminder.Id;
    }

    public async Task<List<ReminderListViewDTO>> GetUserRemindersAsync(Guid userId)
    {
        var remindersView = await _reminderRepository.GetRemindersByUserIdAsync(userId);
        return remindersView;
    }

    public async Task<ReminderInfoViewDTO> GetReminderInfoAsync(Guid reminderId, Guid userId)
    {
        var reminderInfo = await _reminderRepository.GetReminderInfoAsync(reminderId, userId);

        if (reminderInfo == null)
            throw new NotValidReminderException();
        
        return reminderInfo;
    }

    public async Task<List<ReminderListViewDTO>> GetUserReminderByFilterAsync(
        Guid userId,
        ReminderSearchDTO searchDto
    )
    {
        var reminderQuery = _reminderRepository.CreateReminderQuery(userId, searchDto);
        var reminders = await _reminderRepository.GetRemindersByQuery(reminderQuery);
        
        return reminders;
    }

    public async Task UpdateReminderAsync(Guid reminderId, Guid userId, ReminderUpdateDTO dto)
    {
        var reminder = await _reminderRepository.GetReminderAsync(reminderId, userId);
        
        if (reminder == null)
            throw new NotValidReminderException();
        
        reminder.CategoryId = dto.CategoryId;
        reminder.Title = dto.Title;
        reminder.Description = dto.Description;
        reminder.Priority = dto.Priority;
        reminder.DueDate = dto.DueDate;
        
        await _reminderRepository.UpdateReminderAsync(reminder);
    }
}