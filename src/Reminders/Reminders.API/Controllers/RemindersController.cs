using System.Security.Claims;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Reminder;

namespace Reminders.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class RemindersController : Controller
{
    private readonly IReminderService _reminderService;
    private readonly IMapper _mapper;

    public RemindersController(IReminderService reminderService, IMapper mapper)
    {
        _reminderService = reminderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] ReminderRequestDTO dto)
    {
        var createDto = _mapper.Map<ReminderCreateDTO>(dto);
        createDto.CreatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var reminderId = await _reminderService.CreateReminderAsync(createDto);
        
        return Ok(new {ReminderId = reminderId});
    }

    [HttpGet]
    public async Task<ActionResult<List<ReminderViewDTO>>> GetUserReminders()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var reminders = await _reminderService.GetUserRemindersAsync(userId);

        return Ok(reminders);
    }
}