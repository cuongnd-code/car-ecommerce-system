using ManageCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{


    [Route("admin")]
    public class AdminController : Controller
    {


        private readonly ILogger<AdminController> _logger;
        private readonly AppDbContext _context;

        public AdminController(ILogger<AdminController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }


        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {


            return View("DashBroad");
        }


        [HttpGet("orderlist")]
        public IActionResult Order()
        {


            return View();
        }

    }
}
