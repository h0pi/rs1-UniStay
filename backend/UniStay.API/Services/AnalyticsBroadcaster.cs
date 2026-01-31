using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;

public class AnalyticsBroadcaster
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<AnalyticsHub> _hub;

    public AnalyticsBroadcaster(
        ApplicationDbContext db,
        IHubContext<AnalyticsHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    // 🔑 OVA METODA TI FALI
    public async Task BroadcastAsync(int activeUsers)
    {
        await _hub.Clients.All.SendAsync("AnalyticsUpdated", new
        {
            activeUsers,
            totalUsers = await _db.User.CountAsync(),
            totalMessages = await _db.Message.CountAsync()
        });
    }
}

