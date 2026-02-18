 using System.Security.Claims;
using ManageCars.Models;
using ManageCars.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManageCars.Controllers
{

	[Route("shop")]
	//[Authorize(Roles ="User")]
	public class ShopController : Controller
	{

			private readonly  ILogger<ShopController> _logger;
			private readonly AppDbContext _context;

		public ShopController(ILogger<ShopController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}
		[HttpGet("shopping")]
		public IActionResult Shopping()
		{
			return View("Shop");
		}

		[HttpPost("car-list")]
		public JsonResult GetCars([FromBody] PagingRequest pagingRequest)
		{
			_logger.LogInformation("CAR LIST: " + pagingRequest._pageNumber);
			_logger.LogInformation("CAR LISTSIze: " + pagingRequest._pageSize);
			// find all category of car

			var carsCategoryNav = _context.CarCategorys
									.Select( a =>new
									{
										id = a.Id,
										name = a.Name,
									})
									.ToList();

			var cars = _context.Cars
				.Include(c => c.Category) // Include the related Category entity
				.Select(c => new
				{
					Id = c.Id,
					Name = c.Name,
					Year = c.Year,
					Type = c.Category.Name,
					Price = c.Price,
					Image = c.Image,
				})
				.OrderBy(p => p.Id)
				.Skip((pagingRequest._pageNumber - 1) * pagingRequest._pageSize)
				.Take(pagingRequest._pageSize)
				.ToList();
			int totalItems = _context.Cars.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / pagingRequest._pageSize);


			return Json(new
			{
				Categories = carsCategoryNav,
				Cars = cars,
				CurrentPage = pagingRequest._pageNumber,
				TotalPage = totalPages,
			});
		}


		[HttpGet("car/{carId}")]		
		public IActionResult Car(int carId)
		{
			var car = _context.Cars
				.Include(a => a.Category)
				.FirstOrDefault(a => a.Id ==carId);
            ViewBag.Successful = "Deposit  for car: ";
            return View(car);
		}

		//[Authorize]
		//[HttpPost("deposit")]
		//public IActionResult deposit([FromBody] int carId)
		//{
  //          var car = _context.Cars
  //              .Include(a => a.Category)
  //              .FirstOrDefault(a => a.Id == carId);
  //          var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
  //          var accounts = _context.Accounts
		//					.Include(u => u.user)
		//					.FirstOrDefault(a=> a.Id==accountId);
		//	if(car == null || accounts == null)
  //          {
  //              return NotFound();
  //          }
  //          _logger.LogInformation("Deposit for car: " + car.Name);	
  //          _logger.LogInformation("User: " + accounts.user.UserName);

  //          var order = new Order
		//	{
  //              Id = Guid.NewGuid().ToString(),
  //              CarId = carId,
  //              Car = car,
  //              UserId = accounts.user.Id, // Assuming you have a UserId claim
  //              User = accounts.user, // Assuming you have a UserId claim
  //              Quantity = 1, // Default quantity is 1
  //              Status = OrderStatus.Pending, // Default status is "Pending"
  //              TotalPrice = car.Price ?? 0 // Total price of the order, calculated based on quantity and car price

  //          };
  //          _context.Orders.Add(order);
		//	_context.SaveChanges();
  //          return Ok(new {Message= $"Deposit successful for car: {car.Name}" });
		//}

		[HttpPost("car-list-category")]
		public JsonResult Car_category([FromBody] int categoryId)
		{

			//var car_category = _context.Cars
			//					.Where(a => a.CategoryId = category_id)

			_logger.LogInformation("category: ++++=" + categoryId);

			var cars = _context.Cars
							.Where(c => c.CategoryId == categoryId)
							.Select(c => new
							{
								Id = c.Id,
								Name = c.Name,
								Year = c.Year,
								Type = c.Category.Name,
								Price = c.Price,
								Image = c.Image,
							})
							.OrderBy(p => p.Id)
							.ToList();
			return Json(new {
				Cars = cars
			});

		}


		[HttpPost("car-recommend")]
		public JsonResult CarRecommend()
		{
			var cars = _context.Cars
							.OrderByDescending(c => c.DateTimeAdd)
							.Take(5)
							.Select(c => new
							{
								Id = c.Id,
								Name = c.Name,
								Year = c.Year,
								Type = c.Category.Name,
								Price = c.Price,
								Image = c.Image,
								Description = c.CarDetail.Description

							})
							.ToList();



			return Json(new
			{
				Cars = cars
			});

		}




	}

}
