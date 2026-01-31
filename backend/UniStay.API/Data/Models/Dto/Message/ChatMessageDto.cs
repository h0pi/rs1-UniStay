public class ChatMessageDto
{
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}