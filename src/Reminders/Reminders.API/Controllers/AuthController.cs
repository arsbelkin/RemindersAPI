using Microsoft.AspNetCore.Mvc;
using Reminders.Application.Services.Interfaces;

namespace Reminders.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    public Task<IActionResult> Register()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}