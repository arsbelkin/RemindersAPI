namespace Reminders.Application.Common.Interfaces;

public interface IHasher
{
    public string CalculateHash(string password);
}