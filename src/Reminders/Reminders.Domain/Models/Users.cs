namespace Reminders.Domain.Models;

public class Users
{
    public Users()
    {
    }
    
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }
    
    public DateTime CreatedTime { get; set; }
}
