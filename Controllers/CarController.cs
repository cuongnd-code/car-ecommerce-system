using ManageCars.Models;
using ManageCars.Models.Request;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManageCars.Controllers
{
	[Route("car")]
	public class CarController : Controller
	{



		private readonly ILogger<CarController> _logger;
		private readonly AppDbContext _context;



		public CarController(ILogger<CarController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}

		[HttpGet()]
		public IActionResult Index()
		{
			return View();
		}


		[HttpGet("add-car")]
		public IActionResult AddCar()
		{
			return View();
		}
		[HttpPost("add-car")]
		public async Task<IActionResult> AddCar(CarViewModel carViewModel)
		{


			if (string.IsNullOrEmpty(carViewModel.Name) || carViewModel.Year <= 0 || carViewModel.Price <= 0)
			{
				ViewBag.ErrorMessage = "All fields are required.";
				return View();
			}

			string filename;


			if (carViewModel.Image != null && carViewModel.Image.Length > 0)
			{

				filename = Path.GetFileName(carViewModel.Image.FileName);
				_logger.LogInformation("Filename: " + filename);
				var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", filename);
				//filemode.create neu chua co file thi tao moi file, neu co thi se ghi de len file cu
				//day la luu file tren server
				using (var stream = new FileStream(filepath, FileMode.Create))
				{
					await carViewModel.Image.CopyToAsync(stream);
				}

			}
			else
			{
				filename = "default.png";
			}

			_context.Cars.Add(new Car
			{
				Name = carViewModel.Name,
				Model = carViewModel.Model,
				Year = carViewModel.Year,
				Price = carViewModel.Price,
				Image = filename,
				CategoryId = carViewModel.CategoryId
			});
			_logger.LogInformation("ADD CAR: {CarName} ", carViewModel.Name);

			_context.SaveChanges();
			ViewBag.SuccessMessage = "Car added successfully.";

			return View();
		}
		[HttpGet("car-list")]

		public IActionResult CarList()
		{
			_logger.LogInformation("CAR LIST:");

			return View();
		}

		[HttpPost("car-edit1")]
		public JsonResult EditCar([FromBody] int carId)
		{
			var car = _context.Cars.FirstOrDefault(c => c.Id == carId);



			return Json(car);
		}

		[HttpPost("car-edit2")]
		public async Task<JsonResult> EditCar([FromForm] CarViewModel carViewModel)
		{
			string filename;


			if (carViewModel.Image != null && carViewModel.Image.Length > 0)
			{

				filename = Path.GetFileName(carViewModel.Image.FileName);
				_logger.LogInformation("Filename: " + filename);
				var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", filename);
				//filemode.create neu chua co file thi tao moi file, neu co thi se ghi de len file cu
				//day la luu file tren server
				using (var stream = new FileStream(filepath, FileMode.Create))
				{
					await carViewModel.Image.CopyToAsync(stream);
				}

			}
			else
			{
				filename = "default.png";
			}
			_context.Cars.Update(new Car
			{

				Id = carViewModel.Id,
				Name = carViewModel.Name,
				Model = carViewModel.Model,
				Year = carViewModel.Year,
				Price = carViewModel.Price,
				Image = filename,
				CategoryId = carViewModel.CategoryId
			});
			_context.SaveChanges();
			string message = "Car added successfully.";
			return Json(message);
		}


		[HttpDelete("delete-car/{carId}")]
		public IActionResult DeleteCar(int carId)
		{

			_logger.LogInformation("DELETE CAR:");

			var car = _context.Cars.Find(carId);
			if (car == null)
			{
				return NotFound("Car not found.");
			}
			_context.Cars.Remove(car);
			_context.SaveChanges();
			_logger.LogInformation("DELETE CAR: {CarId} ", carId);
			return Ok(new { Message = "Car delete successfully" });
		}

        [HttpGet]
        [Route("Detail/{carId}")]
        public IActionResult GetCarDetail(int carId)
        {
			var car = _context.Cars
				.Include(c => c.CarDetail)
				.Include(c => c.Category)
                .FirstOrDefault(c => c.Id == carId);

            if (car == null)
            {
                return NotFound("Car not found.");
            }

			


            return View("CarDetail",car);

        }

	}

}
