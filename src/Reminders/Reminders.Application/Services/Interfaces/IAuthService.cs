using Reminders.Application.TransferModels;
using Reminders.Domain.Models;

namespace Reminders.Application.Services.Interfaces;

public interface IAuthService
{
    public Task<Guid> RegisterUserAsync(UserRegisterDTO registerDto);
    public Task<string> LoginAsync(UserLoginDTO loginDto);
}
