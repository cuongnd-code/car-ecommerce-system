namespace ManageCars.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        public string SenderKey { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    }
}
