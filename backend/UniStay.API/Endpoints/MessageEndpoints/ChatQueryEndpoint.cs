using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data;

[ApiController]
[Route("api/chat")]
public class ChatQueryEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ChatQueryEndpoint(ApplicationDbContext db)
    {
        _db = db;
    }

    // 🔹 Lista razgovora
    [HttpGet("conversations/{userId}")]
    public async Task<IActionResult> GetConversations(int userId)
    {
        var conversations = await _db.Message
            .Include(m=>m.SenderUser)
            .Include(m=>m.ReceiverUser)
            .Where(m => m.SenderUserID == userId || m.ReceiverUserID == userId)
            .Select(m => new
            {
                UserId = m.SenderUserID == userId ? m.ReceiverUserID : m.SenderUserID,
                displayName = m.SenderUserID == userId
                   ? m.ReceiverUser.Username
                   : m.SenderUser.Username
            })
            .Distinct()
            .ToListAsync();

        return Ok(conversations);
    }

    // 🔹 Poruke između dva usera
    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages(int userId, int otherUserId)
    {
        var messages = await _db.Message
            .Include(m=>m.SenderUser)
            .Where(m =>
                (m.SenderUserID == userId && m.ReceiverUserID == otherUserId) ||
                (m.SenderUserID == otherUserId && m.ReceiverUserID == userId))
            .OrderBy(m => m.SentAt)
            .Select(m => new
            {
                SenderId = m.SenderUserID,
                SenderName = m.SenderUser.Username,
                Content = m.MessageText,
                sentAt = m.SentAt
            })
            .ToListAsync();

        return Ok(messages);
    }

    [HttpGet("search-users")]
    public async Task<IActionResult> SearchUsers(string username)
    {
        // Pretražuješ sve korisnike osim same sebe
        var users = await _db.User // Provjeri zove li se tabela Users
            .Where(u => u.Username.Contains(username))
            .Select(u => new {
                userId = u.UserID,
                displayName = u.Username
            })
            .Take(10)
            .ToListAsync();

        return Ok(users);
    }
}