namespace ManageCars.Controllers.Service
{
	public class CarService
	{
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

		public void DeleteCar(int id)
		{
			// Here you would typically delete the car from a database or a collection
			Console.WriteLine($"Car Deleted: ID: {id}");
		}

		public void GetCarDetails(int id)
		{
			
			
			Console.WriteLine($"Car Details: ID: {id}");
		}


	}
}
