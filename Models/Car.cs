namespace ManageCars.Models
{
    public class Car
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int? Year { get; set; }
        public int? Price { get; set; }

        public int? deposit { get; set; }// tien coc truoc

		public string? Description { get; set; }

        public int? CategoryId { get; set; }

        public  string Image { get; set; } = "default.png"; // Default image if none is provided
        public   CarCategorys? Category { get; set; }
        // Navigation property for related entities, if any
    }
}
