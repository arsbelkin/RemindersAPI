using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;

namespace Reminders.NotificationSenderWorker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;
    
    public Worker(
        IServiceScopeFactory scopeFactory,
        ILogger<Worker> logger
    )
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // if (_logger.IsEnabled(LogLevel.Information))
            // {
            //     _logger.LogInformation("SenderWorker running at: {time}", DateTimeOffset.Now);
            // }
            
            await DoWorkAsync(stoppingToken);
            
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var redis = scope.ServiceProvider.GetRequiredService<IRedisNotification>();
        
        var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
        
        var nowSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
        
        var readyMessages = await redis.GetReadyNotificationsAsync(nowSeconds);

        foreach (var readyMessage in readyMessages)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"{readyMessage.ReceiverEmail}");
            }
        }

        if (readyMessages.Count > 0)
        {
            var notificationsId = readyMessages.Select(n => n.NotificationId).ToList();
        
            await notificationRepository.SetSentNotificationsByListIdAsync(notificationsId);
        }
    }
}