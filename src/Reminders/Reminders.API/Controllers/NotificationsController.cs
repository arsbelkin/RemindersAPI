using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;

namespace Reminders.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationsController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNotification([FromBody] NotificationCreateDTO dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var notificationId = await _notificationService.CreateNotificationAsync(dto, userId);

        return Ok(new { NotificationId = notificationId });
    }
}