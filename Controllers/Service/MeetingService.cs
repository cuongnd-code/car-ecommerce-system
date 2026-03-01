using ManageCars.Models;
using ManageCars.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace ManageCars.Controllers.Service
{



	public class MeetingService
	{

		private readonly AppDbContext _context;
		private readonly ILogger<OrderController> _logger;




		public MeetingService(ILogger<OrderController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}
		public async Task<bool> AddMeeting(MeetingRequest meetingRequest)
		{


			if (await CheckMeeting(meetingRequest.OrderId))
			{

				return false;

			}


			var meeting = new Meeting
			{
				Title = meetingRequest.Title,
				StartTime = meetingRequest.StartTime,
				Location = meetingRequest.Location,
				Description = meetingRequest.Description,
				OrderId = meetingRequest.OrderId,
				Status = MeetingStatus.Scheduled
			};

			_context.Meeting.Add(meeting);


			_context.SaveChanges();

			_logger.LogInformation(
	"Meeting Added | Title: {Title}, StartTime: {StartTime}, Location: {Location}, OrderId: {OrderId}, Status: {Status}",
	meeting.Title,
	meeting.StartTime,
	meeting.Location,
	meeting.OrderId,
	meeting.Status
			);


			return true;



		}

		public async Task<bool> CheckMeeting(int Id)
		{

			Meeting? meetCheck = await _context.Meeting.FirstOrDefaultAsync(m => m.OrderId == Id);

			if (meetCheck != null)
			{
				if (meetCheck.Status == MeetingStatus.Scheduled)
				{
					return false;
				}

			}
			return true;

		}


		public async Task CompleteMeeting(int id)
		{
			var meet = await _context.Meeting
									 .FirstOrDefaultAsync(c => c.Id == id);

			if (meet == null)
				return;

			meet.Status = MeetingStatus.Completed;

			await _context.SaveChangesAsync();
		}
		public async Task CancelMeeting(int id)
		{
			var meet = await _context.Meeting
									 .FirstOrDefaultAsync(c => c.Id == id);

			if (meet == null)
				return;

			meet.Status = MeetingStatus.Canceled;

			await _context.SaveChangesAsync();
		}



	}
}
