using System.ComponentModel.DataAnnotations;

namespace ManageCars.Models.ViewModel
{
    public class CarDetailViewModel
    {
        [StringLength(100)]
        public string? Engine { get; set; }

        [StringLength(100)]
        public string? Transmission { get; set; }

        [StringLength(100)]
        public string? DriveType { get; set; }

        [StringLength(100)]
        public string? FuelType { get; set; }

        [StringLength(50)]
        public string? FuelConsumption { get; set; }

        [Range(1, 100)]
        public int? Seats { get; set; }

        [Range(1, 10)]
        public int? DoorCount { get; set; }

        [StringLength(100)]
        public string? ColorInterior { get; set; }

        [StringLength(100)]
        public string? ColorExterior { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }
    }
}
