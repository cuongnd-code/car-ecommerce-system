namespace ManageCars.Models.Request
{
	public class OrderRequest
	{
		public int CarId { get; set; } 
		
		public required string name { get; set; }

        public required string phone { get; set; }

        public string? email { get; set; }

        public string? Message { get; set; }

    }
}
