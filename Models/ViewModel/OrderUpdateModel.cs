namespace ManageCars.Models.ViewModel
{
	public class OrderUpdateModel
	{
		public int id { get; set; }
		public OrderStatus status { get; set; }
		public string? assignedTo { get; set; }
		public decimal? finalPrice { get; set; }
		public decimal? depositAmount { get; set; }
		public string? paymentMethod { get; set; }
		public string? completionNote { get; set; }
	}
}
