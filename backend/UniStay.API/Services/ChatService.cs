using UniStay.API.Data;
using UniStay.API.Data.Models;

public class ChatService
{
    private readonly ApplicationDbContext _db;

    public ChatService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SaveMessage(MessageDto dto)
    {
        var msg = new Messages
        {
            SenderUserID = dto.SenderUserID,
            ReceiverUserID = dto.ReceiverUserID,
            MessageText = dto.MessageText
        };

        _db.Message.Add(msg);
        await _db.SaveChangesAsync();
    }

    public class MessageDto
    {
        public int SenderUserID { get; set; }
        public int ReceiverUserID { get; set; }
        public string MessageText { get; set; } = string.Empty;
    }
}