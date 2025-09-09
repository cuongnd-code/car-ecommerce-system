namespace ManageCars.Models.ViewModel
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public  IFormFile? Image { get; set; } 
        public string? Description { get; set; }

        public int CategoryId { get; set;}

    }
}
