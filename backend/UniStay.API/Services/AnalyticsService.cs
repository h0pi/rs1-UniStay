//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using UniStay.API.Data;

//public class AnalyticsService
//{
//    private readonly ApplicationDbContext _db;
//    private readonly IHubContext<AnalyticsHub> _hub;

//    public AnalyticsService(ApplicationDbContext db, IHubContext<AnalyticsHub> hub)
//    {
//        _db = db;
//        _hub = hub;
//    }

//    public async Task BroadcastAnalyticsAsync(int activeUsers)
//    {
//        var totalUsers = await _db.User.CountAsync();
//        var totalMessages = await _db.Message.CountAsync();

//        await _hub.Clients.All.SendAsync("AnalyticsUpdated", new
//        {
//            activeUsers,
//            totalUsers,
//            totalMessages
//        });
//    }
//}

using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;

public class AnalyticsService
{
    private readonly ApplicationDbContext _db;

    public AnalyticsService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<object> GetAnalyticsAsync()
    {
        return new
        {
            totalUsers = await _db.User.CountAsync(),
            totalMessages = await _db.Message.CountAsync(),
            activeUsers = await _db.MyAuthInfo.CountAsync(x => x.IsLoggedIn)
        };
    }
}