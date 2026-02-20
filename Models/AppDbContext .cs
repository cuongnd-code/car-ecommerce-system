using Microsoft.EntityFrameworkCore;

namespace ManageCars.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{

		}

		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Car> Cars { get; set; } = null!;
		public DbSet<CarCategorys> CarCategorys { get; set; } = null!;


		public DbSet<Order> Orders { get; set; } = null!;

		public DbSet<VisitorLog> VisitorLogs { get; set; } = null!;

		public DbSet<Meeting> Meeting { get; set; } = null!;

		public DbSet<CarDetail> CarDetails { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Car>()
				.HasOne(c => c.Category)
				.WithMany(cg => cg.Cars)
				.HasForeignKey(c => c.CategoryId);

			modelBuilder.Entity<Order>()
				.Property(o => o.Status)
				  .HasConversion(
			v => v.ToString(),                    // Chuyển Enum thành String khi lưu
			v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v) // Chuyển String thành Enum khi đọc
		);
			modelBuilder.Entity<Order>()
				.HasMany(c => c.Meeting)
				.WithOne(cg => cg.Order)
				.HasForeignKey(c => c.OrderId);

			modelBuilder.Entity<CarDetail>()
				.HasOne(cd => cd.Car)
				.WithOne(c => c.CarDetail)
				.HasForeignKey<CarDetail>(cd => cd.CarId);
		}


	}
}
