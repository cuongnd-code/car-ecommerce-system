using ManageCars.Controllers.Service;
using ManageCars.Hubs;
using ManageCars.Models;
using ManageCars.Models.Request;
using ManageCars.Models.ViewModel;
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

		private readonly OrderService orderService;



		public OrderController(ILogger<OrderController> logger, AppDbContext dbContext, IHubContext<CarHub> hubContext)
		{
			_logger = logger;
			_context = dbContext;
			_hubContext = hubContext;
			orderService = new OrderService(_logger, _context);
		}


		[HttpGet()]
		public IActionResult Index()
		{
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
		public async Task<IActionResult> OrderList([FromBody] PagingRequest pagingRequest)
		{
			var orders = await orderService.getOrdersPage(pagingRequest);

			int totalItems = await orderService.CountOrder();
			int totalPages = (int)Math.Ceiling((double)totalItems / pagingRequest._pageSize);

			return Json(new
			{
				orders = orders,
				currentPage = pagingRequest._pageNumber,
				totalPage = totalPages,
				totalItems = totalItems
			});
		}
		[HttpPost("inform")]
		public IActionResult CustomerInformation([FromBody] int orderId)
		{
			var orders = orderService.GetOrderWithCustomerInfor(orderId);

			bool isMeeting = orderService.CheckMeetingStatus(orderId);

			Meeting meet = orderService.GetMeeting(orderId);




			return Json(new
			{
				Orders = orders,
				IsMeeting = isMeeting,
				Meetdetail = meet
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



		[HttpPost("process")]
		public async Task<IActionResult> ProcessingOrder([FromBody] OrderUpdateModel orderModel)
		{

			await orderService.UpdateOrder(orderModel);
			await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", $"New s");

			return Json(new { success = true, message = "update successful" });

		}
	}
}
