namespace ManageCars.Models
{
	public class OrderLog
	{
		public int Id { get; set; }

		// Liên kết Order
		public int OrderId { get; set; }
		public required Order Order { get; set; }

		// Trạng thái thay đổi
		public OrderStatus? OldStatus { get; set; }
		public OrderStatus? NewStatus { get; set; }

		// Thông tin tài chính thay đổi
		public decimal? OldFinalPrice { get; set; }
		public decimal? NewFinalPrice { get; set; }

		public decimal? OldDepositAmount { get; set; }
		public decimal? NewDepositAmount { get; set; }

		public string? OldPaymentMethod { get; set; }
		public string? NewPaymentMethod { get; set; }

		// Người phụ trách
		public string? OldAssignedTo { get; set; }
		public string? NewAssignedTo { get; set; }

		// Ghi chú
		public string? Note { get; set; }

		// Audit
		public string? UpdatedBy { get; set; }   // Admin nào sửa
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}
