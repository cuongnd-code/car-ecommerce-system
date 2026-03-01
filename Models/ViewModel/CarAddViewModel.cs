using System.ComponentModel.DataAnnotations;


namespace ManageCars.Models.ViewModel
{

    public class CarAddViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Car name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Car name is required")]
        [StringLength(100, ErrorMessage = "Model cannot exceed 100 characters")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1950, 2100, ErrorMessage = "Year must be between 1950 and 2100")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, 100000, ErrorMessage = "Stock must be 0 or greater")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public CarDetailViewModel? Detail { get; set; }
    }
}
