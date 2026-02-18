using System.ComponentModel.DataAnnotations;

namespace ManageCars.Models.Request
{
	public class MeetingRequest
	{

		[Required]
		public string? Title { get; set; }
		[Required]
		public DateTime StartTime { get; set; }
		[Required]
		public DateTime EndTime { get; set; }
		public string? Location { get; set; }
		public string? Description { get; set; }
		[Required]
		public int OrderId { get; set; }

	}
}
