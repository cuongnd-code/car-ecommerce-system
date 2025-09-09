namespace ManageCars.Models.Request
{
	public class OrderRequest
	{
		public int CarId { get; set; } 
		public string OrderId { get; set; } = string.Empty;
	}
}
