namespace ManageCars.Models
{
    public class VisitorLog
    {
        public int Id { get; set; }

        public string VisitorId { get; set; } = null!;

        public string? SessionId { get; set; }

        public DateTime VisitTime { get; set; }

        public DateTime LastActiveTime { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}
