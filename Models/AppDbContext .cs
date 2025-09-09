using ManageCars.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ManageCars.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<CarCategorys> CarCategorys { get; set; } = null!;

		public DbSet<User> Users { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Category)
                .WithMany(cg => cg.Cars)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

		}
	}
}
