namespace ManageCars.Models
{
    public class Conversation
    {
        public int Id { get; set; }

        public string SenderKey { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public bool IsClosed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Message> Messages { get; set; } = new();
    }
}
