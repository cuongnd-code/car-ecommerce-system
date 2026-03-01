using ManageCars.Models;
using ManageCars.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ManageCars.Controllers.Service
{
    public class AdminService
    {
        private readonly ILogger<AdminService> _logger;
        private readonly AppDbContext _context;

        public AdminService(ILogger<AdminService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public Task GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return Task.FromResult(orders);
        }


        public async Task<int> CountAllUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> CountAllOrders()
        {
            return await _context.Orders.CountAsync();
        }
        public async Task<int> CountAllPending()
        {
            return await _context.Orders
       .CountAsync(o => o.Status != OrderStatus.Completed &&
            o.Status != OrderStatus.Cancelled);
        }

        public async Task<int> CountAllSales()
        {
            return await _context.Orders
       .CountAsync(o => o.Status == OrderStatus.Completed);
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order with ID {Id} deleted successfully.", id);
            }
            else
            {
                _logger.LogWarning("Order with ID {Id} not found. Deletion failed.", id);
            }
        }


        public async Task<List<MonthlySalesDto>> GetMonthlySales()
        {
            var startMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
                                .AddMonths(-11);

            var endMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
                                .AddMonths(1)
                                .AddTicks(-1);

            var rawData = await _context.Orders
               .Where(o => o.Status == OrderStatus.Completed
            && o.CompletedAt != null
            && o.CompletedAt >= startMonth
            && o.CompletedAt <= endMonth)
               .GroupBy(o => new
               {
                   Year = o.CompletedAt.Value.Year,
                   Month = o.CompletedAt.Value.Month
               })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Total = g.Sum(o => o.FinalPrice)
                })
                .ToListAsync();

            var result = Enumerable.Range(0, 12)
                .Select(i =>
                {
                    var monthDate = startMonth.AddMonths(i);

                    var data = rawData.FirstOrDefault(x =>
                        x.Year == monthDate.Year &&
                        x.Month == monthDate.Month);

                    return new MonthlySalesDto
                    {
                        Year = monthDate.Year,
                        Month = monthDate.Month,
                        Total = data?.Total ?? 0
                    };
                })
                .ToList();

            return result;
        }

    }
}
