using ManageCars.Controllers.Service;
using ManageCars.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{


    [Route("admin")]
    public class AdminController : Controller
    {


        private readonly AdminService adminService;
        public readonly CarService _carService;
        public AdminController(AdminService adminService, CarService _carService)
        {
            this.adminService = adminService;
            this._carService = _carService;

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

        [HttpGet("stats")]
        public async Task<IActionResult> StateBroad(CarAddViewModel carViewModel)
        {

            int countOrder = await adminService.CountAllOrders();

            int countUser = await adminService.CountAllUsers();

            int countSales = await adminService.CountAllSales();

            int countPending = await adminService.CountAllPending();






            return Json(new
            {
                CountOrder = countOrder,
                CountUser = countUser,
                CountSales = countSales,
                CountPending = countPending

            });


        }

        [HttpGet("monthly-sales")]
        public async Task<IActionResult> GetMonthlySales()
        {


            var data = await adminService.GetMonthlySales();
            return Json(data);
        }


        [HttpGet("car-manager")]
        public IActionResult CarManager()
        {
            return View("CarManager");

        }
    }

}