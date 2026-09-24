using Reminders.Application.Enums;
using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;
using Reminders.Domain.Enums;
using Reminders.Domain.Models;

namespace Reminders.DbScannerWorker;

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
        await DoWorkAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(CalculateDelay(), stoppingToken);

            await DoWorkAsync(stoppingToken);
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

        var publisher = scope.ServiceProvider.GetRequiredService<INotificationPublisher>();

        var notifications = await notificationRepository
            .GetComingNotificationsAsync();

        foreach (var notification in notifications)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var message = new NotificationMessageDTO
            {
                ReceiverEmail = notification.Receiver.Email,
                CategoryTitle = notification.Reminder.Category.Title,
                ReminderTitle = notification.Reminder.Title,
                ReminderDescription = notification.Reminder.Description,
                NotificationTime = notification.NotificationTime
            };

            await publisher.SendAsync(new NotificationWrapper
            {
                MessageType = MessageTypes.Add,
                NotificationId = notification.Id,
                NotificationMessage = message
            });

            notification.UpdateNotificationTime();
            notification.IsProcessed = ProcessedStatusTypes.Processed;
        }

        if (notifications.Count > 0)
            await notificationRepository.UpdateNotificationsListAsync(notifications);
    }

    private static TimeSpan CalculateDelay()
    {
        var now = DateTime.Now;

        // var nextHour = new DateTime(
        //     now.Year,
        //     now.Month,
        //     now.Day,
        //     now.Hour,
        //     0,
        //     0
        // ).AddHours(1);

        var nextHour = new DateTime(
            now.Year,
            now.Month,
            now.Day,
            now.Hour,
            now.Minute,
            now.Second
        ).AddMinutes(1);

        return nextHour - now;
    }
}