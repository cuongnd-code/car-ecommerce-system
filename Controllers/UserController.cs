using System.Security.Claims;
using ManageCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageCars.Controllers
{
	public class UserController : Controller
	{

        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        [HttpGet()]
        public IActionResult Profile()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Assuming you have a UserId claim
            var account = _context.Accounts
                .Include( a=> a.user)
                .FirstOrDefault( a => a.Id ==userid); // Replace with actual user ID or logic to get the current user
            if (account == null)
            {
                return NotFound();
            }
            return View("Profile", account);
        }

        
        [HttpPost("edit-profile")]
        [Route("user/edit-profile")]
        public IActionResult EditProfile(User user)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingUser = _context.Accounts
            .Include(a => a.user)
            .FirstOrDefault(a => a.Id == userid);
                if (existingUser == null)
                {
                    return NotFound();
                }

            _logger.LogInformation(user.UserName);
                existingUser.user.UserName = user.UserName;
                existingUser.user.address = user.address;
                existingUser.user.phone = user.phone;

                _context.SaveChanges();
            
            return RedirectToAction("profile","user");
        }
    }
}
