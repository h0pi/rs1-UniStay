using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniStay.API.Data.Models
{
    public class Messages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        public string? Subject { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        [ForeignKey(nameof(SenderUser))]
        public int SenderUserID { get; set; }
        public Users SenderUser { get; set; }

        [ForeignKey(nameof(ReceiverUser))]
        public int ReceiverUserID { get; set; }
        public Users ReceiverUser { get; set; }
    }
}
