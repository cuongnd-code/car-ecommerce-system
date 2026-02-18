using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using ManageCars.Controllers.Service;
using ManageCars.Models;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace ManageCars.Controllers
{

	


	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context;
        LoginService loginService = new LoginService();


        public HomeController(ILogger<HomeController> logger,AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
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

		//[HttpPost()]
  //      public IActionResult Register(RegisterViewModel registerViewModel)
  //      {

  //          _logger.LogInformation($"AccountName: {registerViewModel.AccountName}");
  //          ViewBag.ErrorMessage = "";

  //          if (string.IsNullOrEmpty(registerViewModel.AccountName)
  //              || string.IsNullOrEmpty(registerViewModel.AccountPassword)
  //              || string.IsNullOrEmpty(registerViewModel.ReAccountPassword))
  //          {
  //              ViewBag.ErrorMessage = "All fields are required.";
  //              return View();
  //          }
  //          string accountName = registerViewModel.AccountName;
  //          string password = registerViewModel.AccountPassword;
  //          string repassword = registerViewModel.ReAccountPassword;
  //          //if (!Regex.IsMatch(registerViewModel.AccountPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\w).{8,}$"))
  //          //{
  //          //    ViewBag.ErrorMessage = "Passwords need to have 1 upperletter, 1 ky tu, 1 number and more than 8.";
  //          //    return View();
  //          //}

  //          // Check if the account already exists
  //          if (_context.Accounts.Any(a => a.AccountName == accountName))
  //          {
  //              ViewBag.ErrorMessage = "Account already exists.";
  //              return View();
  //          }

  //              if (password != repassword)
  //          {
  //              ViewBag.ErrorMessage = "Passwords do not match.";
  //              return View();
  //          }
  //          // Check if passwords match


		//	var account = new Account
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		AccountName = accountName,
		//		AccountPassword = password,
		//		role = "User"
		//	};

		//	var user = new User
		//	{
		//		UserName = accountName,
		//		Account = account 
		//	};
  //          account.user = user; // Set the user property in the account

		//	_context.Accounts.Add(account);
		//	_context.Users.Add(user);
		//	_context.SaveChanges();
  //          _logger.LogInformation("New account created: {AccountName}", account);
  //          return RedirectToAction("login", "Home");
  //      }



		public IActionResult Login()
		{
            return View("Login");
		}
		[HttpPost()]



  //      // async di cung kieu await 
		//public async Task<IActionResult> CheckLogin(string account, string password)
		//{     
  //          var account1 = _context.Accounts.FirstOrDefault(a => a.AccountName == account && a.AccountPassword == password);

  //          _logger.LogInformation(account1.role);

		//	var Claims = new List<Claim>
		//	{
		//		new Claim(ClaimTypes.Name, account),
		//		new Claim(ClaimTypes.NameIdentifier, account1.Id),
		//		new Claim(ClaimTypes.Role, account1.role),  
		//	};


  //          var identity = new ClaimsIdentity(Claims, "MyCookieAuth");
  //          var principal = new ClaimsPrincipal(identity);

		//	await HttpContext.SignInAsync("MyCookieAuth", principal);

  //          return RedirectToAction("shopping","shop");
  //      }

       
        

        public IActionResult Privacy()
        {
            return View();
        }


       

        
    }
}
