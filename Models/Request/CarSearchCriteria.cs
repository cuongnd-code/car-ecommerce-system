namespace ManageCars.Models.Request
{
    public class CarSearchCriteria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? Price { get; set; }
        public int? Seats { get; set; }
        public int? Amount { get; set; }



    }
}
