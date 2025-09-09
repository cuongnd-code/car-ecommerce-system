namespace ManageCars.Models
{
	public class User
	{
	    public int Id { get; set; }
		public required string UserName { get; set; }

		public string? address { get; set; }
		public string? phone { get; set; }	


		public string? AccountId { get; set; }
		public  Account? Account { get; set; } 

		public ICollection<Order> Orders { get; set; } = [];
}
}
