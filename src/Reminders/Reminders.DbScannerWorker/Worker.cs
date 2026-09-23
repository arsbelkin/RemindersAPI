using Reminders.Application.Repositories.Interfaces;
using Reminders.Application.Services.Interfaces;

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
            .GetComingNotificationsAsync(stoppingToken);

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            foreach (var notification in notifications)
            {
                _logger.LogInformation(notification.Title);
                
                await publisher.SendAsync(notification, stoppingToken);
            }
        }
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
        ).AddSeconds(10);

        return nextHour - now;
    }
}