namespace ManageCars.Models
{
	public class Meeting
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public DateTime StartTime { get; set; }
		public string? Location { get; set; }
		public string? Description { get; set; }

		public int OrderId { get; set; }
		public Order? Order { get; set; }

		public MeetingStatus Status { get; set; } // e.g., Scheduled, Completed, Canceled

	}

	public enum MeetingStatus
	{
		Scheduled,
		Completed,
		Canceled
	}

}
