namespace ManageCars.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string? CustomerName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public string? Message { get; set; }

		public int CarId { get; set; }
		public Car? Car { get; set; }


		public int quantity { get; set; } = 1;



		public ICollection<Meeting>? Meeting { get; set; } // Liên kết với bảng Meeting

		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public OrderStatus Status { get; set; }
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
