namespace ManageCars.Models.ViewModel
{
    public class CarListDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
        public CarCategorys Category { get; set; }
        public string Image { get; set; }
    }
}
