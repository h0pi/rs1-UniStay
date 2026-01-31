using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Net;

//[Authorize]
public class ChatHub : Hub
{
    // userId -> connectionId
    private static readonly Dictionary<int, string> Connections = new();
    private static readonly HashSet<int> OnlineUsers = new();

    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userIdStr = httpContext?.Request.Query["userId"];

        if (int.TryParse(userIdStr, out var userId))
        {
            Connections[userId] = Context.ConnectionId;
            Console.WriteLine($"🔗 User {userId} povezan sa {Context.ConnectionId}");
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var item = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
        if (item.Key != 0)
        {
            Connections.Remove(item.Key);
            Console.WriteLine($"❌ User {item.Key} diskonektovan");
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task Typing(int receiverId, int senderId)
    {
        var connectionId = GetConnection(receiverId);
        if (connectionId != null)
        {
            await Clients.Client(connectionId).SendAsync("UserTyping", senderId);
        }
    }

    public static string? GetConnection(int userId)
        => Connections.TryGetValue(userId, out var conn) ? conn : null;

    public static int GetActiveUsers() => OnlineUsers.Count;
}