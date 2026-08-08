using Reminders.Application.TransferModels;

namespace Reminders.Application.Services.Interfaces;

public interface IAuthService
{
    public Task<Guid> RegisterUserAsync(UserRegisterDTO dto);
    public Task<string> LoginAsync(UserLoginDTO dto);
}
