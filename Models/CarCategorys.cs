namespace ManageCars.Models
{
    public class CarCategorys
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        // Navigation property for related cars
        public ICollection<Car> Cars { get; set; } = [];
    }

}
