using Microsoft.AspNetCore.SignalR;


public class AnalyticsHub : Hub {

}
//{
//    private static int ActiveConnections = 0;
//    private readonly AnalyticsBroadcaster _broadcaster;

//    public AnalyticsHub(AnalyticsBroadcaster broadcaster)
//    {
//        _broadcaster = broadcaster;
//    }

//    public override async Task OnConnectedAsync()
//    {
//        ActiveConnections++;

//        await _broadcaster.BroadcastAsync(ActiveConnections);

//        await base.OnConnectedAsync();
//    }

//    public override async Task OnDisconnectedAsync(Exception? exception)
//    {
//        ActiveConnections--;

//        if (ActiveConnections < 0)
//            ActiveConnections = 0;

//        await _broadcaster.BroadcastAsync(ActiveConnections);

//        await base.OnDisconnectedAsync(exception);
//    }

//    // ⬇⬇⬇ OVO JE KLJUČNO ⬇⬇⬇
//    public static int GetActiveUserCount()
//    {
//        return ActiveConnections;
//    }
//}

