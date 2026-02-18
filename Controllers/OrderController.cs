using ManageCars.Hubs;
using ManageCars.Models;
using ManageCars.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ManageCars.Controllers
{

    [Route("order")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly AppDbContext _context;
        private readonly IHubContext<CarHub> _hubContext;



        public OrderController(ILogger<OrderController> logger, AppDbContext dbContext, IHubContext<CarHub> hubContext)
        {
            _logger = logger;
            _context = dbContext;
            _hubContext = hubContext;
        }


        [HttpGet()]
        public IActionResult Index()
        {
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
        //public int Id { get; set; }
        //public string? CustomerName { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string? Email { get; set; }
        //public string? Message { get; set; }

        //public int CarId { get; set; }
        //public Car? Car { get; set; }


        //public int quantity { get; set; } = 1;



        //public ICollection<Meeting>? Meeting { get; set; } // Liên kết với bảng Meeting

        //public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public OrderStatus Status { get; set; } // Trạng thái mặc định
        [HttpPost("list")]
        public IActionResult OrderList([FromBody] PagingRequest pagingRequest)
        {
            var orders = _context.Orders
                .OrderByDescending(o => o.Id) // hoặc o.Id
                .Select(o => new
                {
                    id = o.Id,
                    customerName = o.CustomerName,
                    //phoneNumber = o.PhoneNumber,
                    //emaiil = o.Email,
                    //message=o.Message,
                    carId = o.CarId,
                    quantity = o.quantity,
                    lastmeeting = o.Meeting.OrderByDescending(m => m.StartTime)
                                            .FirstOrDefault(),
                    createdDate = o.CreatedDate,
                    status = o.Status

                })

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
                TotalItems = totalItems
            });
        }
        [HttpPost("inform")]
        public IActionResult CustomerInformation([FromBody] int orderId)
        {
            var orders = _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new
                {
                    customerName = o.CustomerName,
                    phoneNumber = o.PhoneNumber,
                    emaiil = o.Email,
                    message = o.Message

                }).FirstOrDefault();


            return Json(new
            {
                Orders = orders,

            });
        }

        //[HttpGet()]
        //public IActionResult OrderDetail(string id)
        //{
        //          return View(model: id); 
        //}

        //[HttpPost()]
        //public IActionResult OrderInformation([FromBody] string orderRequest)
        //{

        //          var order =_context.Orders

        //		.FirstOrDefault(o => o.Id == orderRequest);


        //	var car = _context.Cars
        //              .Select(c => new
        //              {
        //                  c.Id,
        //                  c.Name,
        //                  c.Model,
        //                  c.Year,
        //                  c.Price,
        //                  c.Image,
        //                  categoryName= c.Category.Name


        //              })
        //		.FirstOrDefault(c => c.Id == 7);
        //          var user = _context.Users
        //              .Select(u => new
        //              {
        //                  u.Id,
        //                  u.UserName,
        //                  u.address,
        //                  u.phone,
        //                  emaill=u.Account.email
        //              })
        //		.FirstOrDefault(u => u.Id == 1);

        //	return Json(new
        //          {
        //		Order = order,
        //		User = user,
        //		Car = car,
        //	});
        //}

        [HttpPost("create")]
        public IActionResult AddOrder(OrderRequest request)
        {

            if (!ModelState.IsValid)
            {

                _logger.LogInformation("New Order Request failed:");

                return Json(new
                {
                    success = false,
                    message = "Please fill in all required fields (Name and Phone)!"
                });
            }


            _context.Add(new Order
            {
                CarId = request.CarId,
                CustomerName = request.name,
                PhoneNumber = request.phone,
                Email = request.email,
                Message = request.Message,
                CreatedDate = DateTime.Now,
                Status = OrderStatus.New
            });



            _logger.LogInformation("Adding new order for CarId: {CarId}, Name: {Name}, Phone: {Phone}", request.CarId, request.name, request.phone);

            _logger.LogInformation("New Order Request Received:");


            _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", $"New order received from {request.name} for Car ID: {request.CarId}");


            _context.SaveChanges();

            return Json(new { success = true, message = "Sent successfully! Our team will call you soon." });


        }

    }
}
