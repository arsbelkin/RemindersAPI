using Reminders.Application.Services.Interfaces;
using Reminders.Infrastructure.Repositories.Interfaces;
using Reminders.Application.TransferModels;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Guid> RegisterUserAsync(UserDTO dto)
    {
        if (dto.Password != dto.PasswordVerify)
            throw new Exception("Password doesn't match");
        
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.Password,
            CreatedTime = DateTime.UtcNow
        };
        
        await _userRepository.CreateUserAsync(user);

        return user.Id;
    }
}