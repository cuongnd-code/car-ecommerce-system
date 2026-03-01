namespace ManageCars.Models
{
	public class Order
	{
		public int Id { get; set; }

		// Customer Info
		public string? CustomerName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public string? Message { get; set; }

		// Car
		public int CarId { get; set; }
		public Car? Car { get; set; }

		public int quantity { get; set; } = 1;

		// Financial
		public decimal? UnitPrice { get; set; }
		public decimal? FinalPrice { get; set; }
		public decimal? DepositAmount { get; set; }
		public decimal? PaidAmount { get; set; }
		public string? PaymentMethod { get; set; }

		// Status
		public OrderStatus Status { get; set; }

		// Internal handling
		public string? AssignedTo { get; set; }

		// Completed
		public DateTime? CompletedAt { get; set; }
		public string? CompletedBy { get; set; }
		public string? CompletionNote { get; set; }

		// Cancelled
		public DateTime? CancelledAt { get; set; }
		public string? CancelReason { get; set; }
		public string? CancelledBy { get; set; }

		public ICollection<Meeting>? Meeting { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}

	public enum OrderStatus
	{
		New = 0,
		Contacted = 1,
		Deposit = 2,
		ViewingScheduled = 3,
		Negotiating = 4,    // Đang thương lượng giá cả hoặc làm thủ tục trả góp
		Completed = 5,
		Cancelled = 6
	}
}
