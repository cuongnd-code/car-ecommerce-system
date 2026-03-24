using System.Text.RegularExpressions;
using firstWeb.Controllers;
using ManageCars.Models;
using ManageCars.Models.Request;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace ManageCars.Controllers.Service
{
	public class HomeService
	{

		private readonly ILogger<HomeController> _logger;

		private readonly AppDbContext _context;



		public HomeService(ILogger<HomeController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}

		public string GetWelcomeMessage()
		{
			return "Welcome to the Car Management System!";
		}

		public MessageResult RegisterUser(RegisterViewModel model)
		{
			if (model == null)
				return new MessageResult { Success = false, Reason = "Invalid data" };

			if (string.IsNullOrWhiteSpace(model.Username) ||
				string.IsNullOrWhiteSpace(model.Email) ||
				string.IsNullOrWhiteSpace(model.PasswordHash) ||
				string.IsNullOrWhiteSpace(model.ReAccountPassword))
			{
				return new MessageResult
				{
					Success = false,
					Reason = "All fields are required"
				};
			}

			// Username length check
			if (model.Username.Length < 4)
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Username must be at least 4 characters"
				};
			}

			// Email format
			if (!Regex.IsMatch(model.Email,
				@"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Invalid email format"
				};
			}

			// Password strength
			if (!Regex.IsMatch(model.PasswordHash,
				@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Password must contain 1 uppercase, 1 lowercase, 1 number and be at least 8 characters"
				};
			}

			if (model.PasswordHash != model.ReAccountPassword)
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Passwords do not match"
				};
			}

			// Check username exists
			if (_context.Users.Any(u => u.Username == model.Username))
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Username already exists"
				};
			}

			// Check email exists
			if (_context.Users.Any(u => u.Email == model.Email))
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Email already exists"
				};
			}

			// Hash password
			var passwordHasher = new PasswordHasher<User>();
			var user = new User
			{
				Username = model.Username,
				Email = model.Email
			};

			user.PasswordHash = passwordHasher.HashPassword(user, model.PasswordHash);

			_context.Users.Add(user);
			_context.SaveChanges();

			return new MessageResult
			{
				Success = true,
				Reason = "Register successful"
			};
		}


		public MessageResult Login(LoginViewModel model)
		{
			if (model == null ||
				string.IsNullOrWhiteSpace(model.Username) ||
				string.IsNullOrWhiteSpace(model.Password))
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Username and password are required"
				};
			}

			var user = _context.Users
				.FirstOrDefault(u => u.Username == model.Username);

			if (user == null)
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Invalid username or password"
				};
			}

			var passwordHasher = new PasswordHasher<User>();
			var result = passwordHasher.VerifyHashedPassword(
				user,
				user.PasswordHash,
				model.Password);

			if (result == PasswordVerificationResult.Failed)
			{
				return new MessageResult
				{
					Success = false,
					Reason = "Invalid username or password"
				};
			}






			return new MessageResult
			{
				Success = true,
				Reason = "Login successful",
				id = user.Id
			};
		}




	}
}
