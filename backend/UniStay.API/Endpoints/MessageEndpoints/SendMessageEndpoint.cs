using Microsoft.AspNetCore.Mvc;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.ComponentModel;

[ApiController]
[Route("api/messages")]
public class SendMessageEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<ChatHub> _hub;
    private readonly AnalyticsBroadcaster _analytics;
    private readonly AnalyticsHub _analyticsHub;

    public SendMessageEndpoint(ApplicationDbContext db, IHubContext<ChatHub> hub, AnalyticsBroadcaster analytics)
    {
        _db = db;
        _hub = hub;
        _analytics = analytics;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] MessageDto dto)
    {
        try {

            Console.WriteLine(">>> Primljen DTO: " + dto.MessageText); 

            var msg = new Messages
            {
                SenderUserID = dto.SenderUserID,
                ReceiverUserID = dto.ReceiverUserID,
                MessageText = dto.MessageText,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Message.Add(msg);
            await _db.SaveChangesAsync();
            //var activeUsers = await _db.MyAuthInfo.CountAsync(x => x.IsLoggedIn);

            //await _analyticsHub.Clients.All.SendAsync("AnalyticsUpdated", new
            //{
            //    activeUsers,
            //    totalUsers = await _db.User.CountAsync(),
            //    totalMessages = await _db.Message.CountAsync()
            //});

            // realtime push
            var sender = await _db.User
                .Where(u => u.UserID == dto.SenderUserID)
                .Select(u => new {
                    u.UserID,
                    DisplayName = u.Username
                })
                .FirstAsync();

            if (sender == null)
            {
                return BadRequest("Sender not found");
            }

            //await _hub.Clients.User(dto.ReceiverUserID.ToString())
            var receiverConnection = ChatHub.GetConnection(dto.ReceiverUserID);

            if (receiverConnection != null)
            {
                await _hub.Clients.Client(receiverConnection)
                    .SendAsync("ReceiveMessage", new
                    {
                        senderId = sender.UserID,
                        senderName = sender.DisplayName,
                        content = dto.MessageText
                    });
            }

            return Ok(msg);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Greska na serveru: {ex.Message}");
        }
        
        }
    /*

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto dto)
    {
        // 1. Spasi u bazu...

        // 2. POŠALJI PREKO SIGNALR-A
        // Budući da nemamo [Authorize], šaljemo svima, a Angular će filtrirati 
        // (za faks je ovo najlakše, iako nije idealno za produkciju)
        await _hub.Clients.All.SendAsync("ReceiveMessage", new
        {
            senderId = dto.SenderUserID,
            senderName = "User " + dto.SenderUserID, // Možeš povući pravo ime iz baze
            content = dto.MessageText
        });

        return Ok();
    }*/

    public class MessageDto
    {
        public int SenderUserID { get; set; }
        public int ReceiverUserID { get; set; }
        public string MessageText { get; set; } = string.Empty;
    }
}