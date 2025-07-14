using Microsoft.AspNetCore.Mvc;

namespace firstWeb.Controllers
{
    public class MovieController : Controller
    {


        public IActionResult AddMovie()
        {
            return View();
        }
       
    }
}
