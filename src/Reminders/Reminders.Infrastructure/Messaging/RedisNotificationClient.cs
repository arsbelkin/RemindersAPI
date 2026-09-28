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

    public async Task AddToQueueAsync(NotificationWrapper message)
    {
        long score = ((DateTimeOffset)message.NotificationMessage.NotificationTime.ToUniversalTime()).ToUnixTimeSeconds();
        await _redis.SortedSetAddAsync(QueueKey, message.NotificationId.ToString(), score);
    }
}