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
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            
            var userRep =  scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await userRep.GetUserByUsernameOrEmailAsync("arseniibelkin@gmail.com");
            
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation($"User: {user?.Email}");
            }

            await Task.Delay(10_000, stoppingToken);
        }
    }
}