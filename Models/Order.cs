using System.ComponentModel.DataAnnotations;

namespace ManageCars.Models
{
	public class Order
	{
		public required string Id { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public int CarId { get; set; }
		public Car? Car { get; set; } // Navigation property to the Car entity

		public int UserId { get; set; }
		public User? User { get; set; } // Navigation property to the User entity
		public int Quantity { get; set; } = 1; // Default quantity is 1
		public OrderStatus Status { get; set; }
		public decimal TotalPrice { get; set; } // Total price of the order, calculated based on quantity and car price
		
	}

	public enum OrderStatus
	{
		[Display(Name = "Pending")]
		Pending,
		[Display(Name = "Deposit")]
		Deposit,
		[Display(Name = "Transfer")]
		Transfer,
		[Display(Name = "Paid")]
		Paid,
		[Display(Name = "Failed")]
		Failed
	}
}
