using ManageCars.Models;
using ManageCars.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageCars.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly AppDbContext _context;
        public OrderController(ILogger<OrderController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }


        [HttpGet()]
        public IActionResult Index() {
            _logger.LogInformation("Order Page Accessed");
            return View();
        }

		[HttpPost()]
		public IActionResult OrderUser()
		{


			return View();
		}
		[HttpGet()]
        public IActionResult ManagerOrder()
        {


            return View();
        }

        [HttpPost()]
        public IActionResult OrderList([FromBody] PagingRequest pagingRequest)
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Select(o => new
                {
                    Id = o.Id,
                    OrderDate=o.OrderDate,
					Quantity = o.Quantity,
                    Status= o.Status,
                    TotalPrice=o.TotalPrice,
                    CarId = o.CarId,
                    UserId = o.UserId,
                }
                
                )
				.Skip(pagingRequest._pageSize * (pagingRequest._pageNumber - 1))
                .Take(pagingRequest._pageSize)
                .ToList();
            int totalItems = _context.Orders.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pagingRequest._pageSize);
            return Json(new
            {
                Orders = orders,
                CurrentPage = pagingRequest._pageNumber,
                TotalPage = totalPages,
            });
        }

		[HttpGet()]
		public IActionResult OrderDetail(string id)
		{
            return View(model: id); 
		}

		[HttpPost()]
		public IActionResult OrderInformation([FromBody] string orderRequest)
		{

            var order =_context.Orders
				
				.FirstOrDefault(o => o.Id == orderRequest);


			var car = _context.Cars
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Model,
                    c.Year,
                    c.Price,
                    c.deposit,
                    c.Description,
                    c.Image,
                    categoryName= c.Category.Name
                    
                    
                })
				.FirstOrDefault(c => c.Id == 7);
            var user = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.address,
                    u.phone,
                    emaill=u.Account.email
                })
				.FirstOrDefault(u => u.Id == 1);

			return Json(new
            {
				Order = order,
				User = user,
				Car = car,
			});
		}

	}
}
