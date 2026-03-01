
using System.ComponentModel.DataAnnotations.Schema;
namespace ManageCars.Models
{
    public class Car
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Model { get; set; }

        public int Stock { get; set; } = 0;  // so luong xe hien co
        public int Year { get; set; } = 0;


        [Column(TypeName = "decimal(18,2)")] // Nên dùng 18,2 cho tiền tệ để tránh tràn số
        public decimal Price { get; set; } = 0;

        public DateTime? DateTimeAdd { get; set; }
        public int CategoryId { get; set; } = 0;

        public string Image { get; set; } = "default.png";
        public CarCategorys? Category { get; set; }


        public CarDetail? CarDetail { get; set; }

        public ICollection<Order>? Orders { get; set; }


    }
}
