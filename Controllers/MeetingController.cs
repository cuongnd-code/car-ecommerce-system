using ManageCars.Controllers.Service;
using ManageCars.Models;
using ManageCars.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{

	[Route("meeting")]

	public class MeetingController : Controller
	{
		private readonly AppDbContext _context;
		private readonly ILogger<OrderController> _logger;
		private readonly MeetingService meetingService;




		public MeetingController(ILogger<OrderController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
			meetingService = new MeetingService(_logger, _context);
		}


		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Meetinglist()
		{
			return View();
		}
		[HttpPost()]
		public IActionResult Meetinglist(int id)
		{
			return View();
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddMeeting(MeetingRequest meetingRequest)
		{


			if (await meetingService.AddMeeting(meetingRequest))
			{

				return Json(new { success = true, message = "Meeting scheduled successfully!" });
			}
			else
			{
				return Json(new { success = false, message = "Meeting for this order is already scheduled!" });


			}



		}








	}
}
