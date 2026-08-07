using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.Common.Interfaces;
using Reminders.Application.Common;
using Reminders.Infrastructure.Repositories.Interfaces;
using Reminders.Application.TransferModels;
using Reminders.Domain.Models;

namespace Reminders.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;
    
    public AuthService(
        IUserRepository userRepository, 
        IHasher hasher,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _configuration = configuration;
    }
    
    public async Task<Guid> RegisterUserAsync(UserRegisterDTO dto)
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

    public async Task<string> LoginAsync(UserLoginDTO dto)
    {
        var user = await _userRepository.GetUserLoginAsync(dto.InputString);
        
        if (user is null)
            throw new Exception("not found");

        var token = GenerateJWTToken(user);
        return token;
    }

    private string GenerateJWTToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(2),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}