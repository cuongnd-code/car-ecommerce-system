using System.Diagnostics;
using System.Runtime.CompilerServices;
using firstWeb.Controllers.Service;
using firstWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace firstWeb.Controllers
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
        [HttpGet("Register")]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost("Register")]
        public IActionResult Register(String account,String password, String repassword)
        {
            if(string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repassword))
            {
                ViewBag.ErrorMessage = "All fields are required.";
                return View();
            }
            if(password != repassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }
            _context.Accounts.Add(new Account
            {
                Id = Guid.NewGuid().ToString(), // Generate a new unique ID
                AccountName = account,
                AccountPassword = password,
                Name = "",
                Email = ""
            });
            _context.SaveChanges();
            return RedirectToAction("Login","Home");
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckLogin()
        {


            return View();
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
