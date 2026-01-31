using Microsoft.AspNetCore.SignalR;

public class AnalyticsBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<AnalyticsHub> _hub;

    public AnalyticsBackgroundService(
        IServiceScopeFactory scopeFactory,
        IHubContext<AnalyticsHub> hub)
    {
        _scopeFactory = scopeFactory;
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var analytics = scope.ServiceProvider.GetRequiredService<AnalyticsService>();

            var data = await analytics.GetAnalyticsAsync();
            await _hub.Clients.All.SendAsync("AnalyticsUpdated", data);

            await Task.Delay(5000, stoppingToken);
        }
    }
}