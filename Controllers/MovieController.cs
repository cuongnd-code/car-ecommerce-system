using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{
    public class MovieController : Controller
    {


        public IActionResult AddMovie()
        {
            return View();
        }
       
    }
}
