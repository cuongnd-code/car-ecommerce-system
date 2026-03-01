using ManageCars.Models;
using ManageCars.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ManageCars.Controllers.Service
{
    public class CarService
    {

        private readonly ILogger<CarService> _logger;
        private readonly AppDbContext _context;

        public CarService(ILogger<CarService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public void AddCar(string name, string model, string color, int year, int price, string description)
        {
            // Here you would typically add the car to a database or a collection
            Console.WriteLine($"Car Added: {name}, Model: {model}, Color: {color}, Year: {year}, Price: {price}, Description: {description}");
        }


        public void UpdateCar(int id, string name, string model, string color, int year, int price, string description)
        {
            // Here you would typically update the car in a database or a collection
            Console.WriteLine($"Car Updated: ID: {id}, Name: {name}, Model: {model}, Color: {color}, Year: {year}, Price: {price}, Description: {description}");
        }

        public void DeleteCar(Car car)
        {

            _context.Cars.Remove(car);
            _context.SaveChanges();

        }

        public void GetCarDetails(int id)
        {


            Console.WriteLine($"Car Details: ID: {id}");
        }
        public async Task<List<CarListDto>> GetCarsAsync()
        {
            return await _context.Cars
                .Include(c => c.Category)
                .Select(c => new CarListDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    Stock = c.Stock,
                    Category = c.Category,
                    Image = c.Image
                })
                .ToListAsync();
        }


        public async Task<Car> GetCarById(int Id)
        {
            return await _context.Cars.FirstOrDefaultAsync(c => c.Id == Id);
        }
        public Car GetCarDeleteById(int Id)
        {
            return _context.Cars.FirstOrDefault(c => c.Id == Id);
        }


        public Car GetCarDetailCategory(int carId)
        {


            return _context.Cars
                .Include(c => c.CarDetail)
                .Include(c => c.Category)
                .FirstOrDefault(c => c.Id == carId);
        }

        public async Task AddCar(CarAddViewModel carViewModel)
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
            var car = new Car
            {
                Name = carViewModel.Name,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                Price = carViewModel.Price,
                Image = filename,
                CategoryId = carViewModel.CategoryId
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();


            if (carViewModel.Detail != null)
            {
                var detail = new CarDetail
                {
                    CarId = car.Id,
                    Engine = carViewModel.Detail.Engine,
                    Transmission = carViewModel.Detail.Transmission,
                    DriveType = carViewModel.Detail.DriveType,
                    FuelType = carViewModel.Detail.FuelType,
                    FuelConsumption = carViewModel.Detail.FuelConsumption,
                    Seats = carViewModel.Detail.Seats,
                    DoorCount = carViewModel.Detail.DoorCount,
                    ColorInterior = carViewModel.Detail.ColorInterior,
                    ColorExterior = carViewModel.Detail.ColorExterior,
                    Description = carViewModel.Detail.Description
                };
                _context.CarDetails.Add(detail);


                await _context.SaveChangesAsync();




            }
        }


        public async Task UpdateCarDetail(CarAddViewModel carViewModel)
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


            var car = new Car
            {
                Name = carViewModel.Name,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                Price = carViewModel.Price,
                Image = filename,
                CategoryId = carViewModel.CategoryId
            };

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            if (carViewModel.Detail != null)
            {
                var detail = new CarDetail
                {
                    CarId = car.Id,
                    Engine = carViewModel.Detail.Engine,
                    Transmission = carViewModel.Detail.Transmission,
                    DriveType = carViewModel.Detail.DriveType,
                    FuelType = carViewModel.Detail.FuelType,
                    FuelConsumption = carViewModel.Detail.FuelConsumption,
                    Seats = carViewModel.Detail.Seats,
                    DoorCount = carViewModel.Detail.DoorCount,
                    ColorInterior = carViewModel.Detail.ColorInterior,
                    ColorExterior = carViewModel.Detail.ColorExterior,
                    Description = carViewModel.Detail.Description
                };
                _context.CarDetails.Update(detail);

                await _context.SaveChangesAsync();


            }
        }



    }
}
