using ManageCars.Controllers.Service;
using ManageCars.Hubs;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ManageCars.Controllers
{
    [Route("car")]
    public class CarController : Controller
    {



        private readonly IHubContext<CarHub> _hubContext;


        public readonly CarService _carService;

        public CarController(CarService _carService, IHubContext<CarHub> _hubContext)
        {

            this._carService = _carService;
            this._hubContext = _hubContext;

        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }

        [HttpGet("add-car")]
        public IActionResult AddCar()
        {
            return View();
        }




        [HttpPost("add-car")]
        public async Task<IActionResult> AddCar(CarAddViewModel carViewModel)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }



            _carService.AddCar(carViewModel);

            await _hubContext.Clients.All.SendAsync("AddCarNotification", "New car add ");



            ViewBag.SuccessMessage = "Car added successfully.";

            return View();
        }
        [HttpGet("car-list")]

        public IActionResult CarList()
        {



            return View();
        }



        [HttpPost("car-edit")]
        public async Task<IActionResult> Edit([FromForm] CarAddViewModel carViewModel)
        {




            if (!ModelState.IsValid)
            {
                return View(carViewModel);
            }
            _carService.UpdateCarDetail(carViewModel);


            await _hubContext.Clients.All.SendAsync("AddCarNotification", "a Car Have been update ");

            ViewBag.SuccessMessage = "Car update successfully.";

            return View();
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carService.GetCarById(id);

            if (car == null)
                return NotFound();

            var model = new CarAddViewModel
            {
                Id = car.Id,
                Name = car.Name,
                Model = car.Model,
                Year = car.Year,
                Price = car.Price,
                Stock = car.Stock,
                CategoryId = car.CategoryId,
                Description = car.CarDetail?.Description,

                Detail = new CarDetailViewModel
                {
                    Engine = car.CarDetail?.Engine,
                    Transmission = car.CarDetail?.Transmission,
                    DriveType = car.CarDetail?.DriveType,
                    FuelType = car.CarDetail?.FuelType,
                    FuelConsumption = car.CarDetail?.FuelConsumption,
                    Seats = car.CarDetail?.Seats,
                    DoorCount = car.CarDetail?.DoorCount,
                    ColorInterior = car.CarDetail?.ColorInterior,
                    ColorExterior = car.CarDetail?.ColorExterior,
                    Description = car.CarDetail?.Description
                }
            };

            return View(model);
        }


        [HttpDelete("delete-car/{carId}")]
        public async Task<IActionResult> DeleteCar(int carId)
        {


            var car = _carService.GetCarById(carId);
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            await _carService.GetCarById(carId);
            return Ok(new { Message = "Car delete successfully" });
        }



        [HttpGet]
        [Route("Detail/{carId}")]
        public IActionResult GetCarDetail(int carId)
        {
            var car = _carService.GetCarDetailCategory(carId);

            if (car == null)
            {
                return NotFound("Car not found.");
            }


            return View("CarDetail", car);

        }

    }

}
