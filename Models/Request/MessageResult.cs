namespace ManageCars.Models.Request
{
    public class MessageResult
    {
        public bool Success { get; set; }
        public string? Reason { get; set; }

        public Guid? id { get; set; }

    }
}
