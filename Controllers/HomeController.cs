using System.Security.Claims;
using ManageCars.Controllers.Service;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace firstWeb.Controllers
{
	public class HomeController : Controller
	{

		private readonly HomeService _homeService;

		public HomeController(HomeService homeService)
		{

			_homeService = homeService;
		}
		[HttpGet("Register")]
		public IActionResult Register()
		{

			return View();
		}
		[HttpPost("Register")]
		public IActionResult Register(RegisterViewModel model)
		{
			var result = _homeService.RegisterUser(model);

			if (!result.Success)
			{
				ViewBag.ErrorMessage = result.Reason;
				return View(model);
			}

			return RedirectToAction("Login", "Home");
		}

		[HttpPost("Login")]
		public IActionResult Login(LoginViewModel model)
		{
			var result = _homeService.Login(model);

			if (!result.Success)
			{
				TempData["Error"] = result.Reason;
				return View(model);
			}

			var claims = new List<Claim>
	{
		new Claim(ClaimTypes.Name, model.Username),
		new Claim(ClaimTypes.NameIdentifier, result.id.ToString()),
		new Claim(ClaimTypes.Role, "User")
	};


			return RedirectToAction("Index", "Home");
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}


		//[HttpPost]
		//public IActionResult? Home(String account, String password)
		//{


		//    if(!loginService.IsValidUser(account, password))
		//    {
		//        Console.WriteLine("login failed");

		//        return View("Login");
		//    }
		//    Console.WriteLine("login success");

		//    return View();


		//}


	}
}
