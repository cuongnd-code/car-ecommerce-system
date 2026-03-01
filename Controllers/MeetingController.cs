using ManageCars.Controllers.Service;
using ManageCars.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{

    [Route("meeting")]

    public class MeetingController : Controller
    {

        private readonly MeetingService meetingService;

        public MeetingController(MeetingService meetingService)
        {
            this.meetingService = meetingService;
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

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteMeeting([FromBody] int id)
        {

            await meetingService.CompleteMeeting(id);

            return Json(new { success = true, message = "Meeting scheduled successfully!" });

        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelMeeting([FromBody] int id)
        {

            await meetingService.CancelMeeting(id);

            return Json(new { success = true, message = "Meeting cancel successfully!" });

        }










    }
}
