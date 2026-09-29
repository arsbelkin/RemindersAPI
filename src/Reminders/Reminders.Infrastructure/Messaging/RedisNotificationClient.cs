using System.Text.Json;
using Reminders.Application.Enums;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;
using StackExchange.Redis;

namespace Reminders.Infrastructure.Messaging;

public class RedisNotificationClient : IRedisNotification
{
    private readonly IDatabase _redis;
    private const string QueueKey = "NotificationQueue";

    public RedisNotificationClient(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task ProcessMessageAsync(NotificationWrapper message)
    {
        switch (message.MessageType)
        {
            case MessageTypes.Add:
                await AddToQueueAsync(message);
                break;
            
            case MessageTypes.Update:
                await UpdateAsync(message);
                break;
            
            case MessageTypes.Delete:
                await DeleteFromQueueAsync(message);
                break;
            
            default:
                break;
        }
    }

    public async Task AddToQueueAsync(NotificationWrapper message)
    {
        var score = ((DateTimeOffset)message.NotificationMessage.NotificationTime
            .ToUniversalTime()).ToUnixTimeSeconds();

        var objectKey = $"notification:{message.NotificationId}";
        
        var jsonValue = JsonSerializer.Serialize(message.NotificationMessage);
        await _redis.StringSetAsync(objectKey, jsonValue, TimeSpan.FromHours(2));
        
        await _redis.SortedSetAddAsync(QueueKey, message.NotificationId.ToString(), score);
    }

    public async Task UpdateAsync(NotificationWrapper message)
    {
        await DeleteFromQueueAsync(message);
        await AddToQueueAsync(message);
    }
    
    public async Task DeleteFromQueueAsync(NotificationWrapper message)
    {
        var objectKey = $"notification:{message.NotificationId}";

        var transaction = _redis.CreateTransaction();
        
        _ = transaction.SortedSetRemoveAsync(QueueKey, message.NotificationId.ToString());
        _ = transaction.KeyDeleteAsync(objectKey);
        
        await transaction.ExecuteAsync();
    }
}