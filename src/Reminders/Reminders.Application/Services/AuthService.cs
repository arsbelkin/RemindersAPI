using Reminders.Application.Services.Interfaces;
using Reminders.Application.Common.Interfaces;
using Reminders.Application.Common;
using Reminders.Infrastructure.Repositories.Interfaces;
using Reminders.Application.TransferModels;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;
    
    public AuthService(IUserRepository userRepository, IHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }
    
    public async Task<Guid> RegisterUserAsync(UserDTO dto)
    {
        if (!EmailValidator.IsValid(dto.Email))
            throw new ArgumentException("Invalid email");
        
        if (dto.Password != dto.PasswordVerify)
            throw new ArgumentException("Password doesn't match");
        
        if (await _userRepository.CheckUserByEmailAsync(dto.Email))
            throw new ArgumentException("Email already exists");
        
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = _hasher.CalculateHash(dto.Password),
            CreatedTime = DateTime.UtcNow
        };
        
        await _userRepository.CreateUserAsync(user);

        return user.Id;
    }
}