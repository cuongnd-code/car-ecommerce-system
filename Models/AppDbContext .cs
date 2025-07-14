using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace firstWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;

    }
}
