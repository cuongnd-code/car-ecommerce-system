
using System.Security.Claims;
using ManageCars.Controllers.Service;
using ManageCars.Models.Request;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{




	public class HomeController : Controller
	{


		private readonly HomeService homeService;



		public HomeController(HomeService homeService)
		{
			this.homeService = homeService;
		}

		public IActionResult Register()
		{

			return View();
		}

		public IActionResult Index()
		{

			return View();
		}


		[HttpGet("error")]
		public IActionResult ThrowError()
		{

			throw new KeyNotFoundException("Xe không ton tai");
		}



		[HttpPost()]
		public IActionResult Register(RegisterViewModel registerViewModel)
		{



			MessageResult result = homeService.RegisterUser(registerViewModel);
			if (!result.Success)
			{
				TempData["Error"] = result.Reason;

				return RedirectToAction("Register");

			}


			TempData["Success"] = result.Reason;



			return RedirectToAction("Register", "Home");
		}

		[HttpPost()]
		public async Task<IActionResult> Login(LoginViewModel login)
		{


			MessageResult result = homeService.Login(login);

			if (!result.Success)
			{
				TempData["Error"] = result.Reason;

				return RedirectToAction("Login");

			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, login.Username!),
				new Claim(ClaimTypes.Role, "User")
			};

			var identity = new ClaimsIdentity(claims, "MyCookieAuth");

			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync("MyCookieAuth", principal);

			TempData["Success"] = result.Reason;



			return RedirectToAction("", "Home");
		}

		[HttpGet()]
		public async Task<IActionResult> Logout(LoginViewModel login)
		{


			await HttpContext.SignOutAsync("MyCookieAuth");

			return RedirectToAction("", "Home");



		}

		[HttpGet()]
		public IActionResult Login()
		{
			return View("Login");
		}
		[HttpPost()]



		// async di cung kieu await 
		//public async Task<IActionResult> CheckLogin(string account, string password)
		//{
		//	var account1 = _context.Accounts.FirstOrDefault(a => a.AccountName == account && a.AccountPassword == password);

		//	_logger.LogInformation(account1.role);

		//	var Claims = new List<Claim>
		//	{
		//		new Claim(ClaimTypes.Name, account),
		//		new Claim(ClaimTypes.NameIdentifier, account1.Id),
		//		new Claim(ClaimTypes.Role, account1.role),
		//	};


		//	var identity = new ClaimsIdentity(Claims, "MyCookieAuth");
		//	var principal = new ClaimsPrincipal(identity);

		//	await HttpContext.SignInAsync("MyCookieAuth", principal);

		//	return RedirectToAction("shopping", "shop");
		//}




		public IActionResult Privacy()
		{
			return View();
		}





	}
}
