using System.ComponentModel.DataAnnotations;

namespace ManageCars.Models
{
	public class CarDetail
	{

		public int Id { get; set; }

		public int CarId { get; set; }

		public Car? Car { get; set; }


		// --- Thông số chi tiết ---
		public string? Engine { get; set; }        // Động cơ
		public string? Transmission { get; set; }  // Hộp số
		public string? DriveType { get; set; }     // Hệ dẫn động

		public string? FuelType { get; set; }      // Loại nhiên liệu
		public string? FuelConsumption { get; set; } // Tiêu hao nhiên liệu

		public int? Seats { get; set; }            // Số chỗ ngồi
		public int? DoorCount { get; set; }        // Số cửa

		public string? ColorInterior { get; set; } // Màu nội thất
		public string? ColorExterior { get; set; } // Màu ngoại thất (thay cho Color ở bảng Car cũ)

		[MaxLength(2000)]
		public string? Description { get; set; }   // Mô tả chi tiết dài

	}
}
