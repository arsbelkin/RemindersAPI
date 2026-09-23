using Reminders.Application.Repositories.Interfaces;

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
            
        var userRep =  scope.ServiceProvider.GetRequiredService<IUserRepository>();

        var user = await userRep.GetUserByUsernameOrEmailAsync("arseniibelkin@gmail.com");
            
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _logger.LogInformation($"User: {user?.Email}");
        }

        await Task.CompletedTask;
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