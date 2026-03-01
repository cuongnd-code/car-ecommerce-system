namespace ManageCars.Models.ViewModel
{
    public class OrderViewModel
    {

        public int id { get; set; }

        public string customerName { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; }

        public string message { get; set; }

        public int carId { get; set; }

        public int quantity { get; set; }

        public DateTime createdDate { get; set; }

        public OrderStatus status { get; set; }

        public Meeting lastmeeting { get; set; }
    }
}
