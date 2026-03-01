using ManageCars.Controllers;

namespace ManageCars.Middleware
{
    public class VisitorTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<CarController> _logger;



        public VisitorTrackingMiddleware(RequestDelegate next, ILogger<CarController> logger)
        {
            _next = next;
            _logger = logger;
        }

        //public async Task InvokeAsync(HttpContext context, AppDbContext db)
        //{
        //    var visitorId = Helper.VisitorHelper.GetOrCreateVisitorId(context);
        //    var sessionId = context.Session.Id;

        //    var log = db.VisitorLogs.FirstOrDefault(v => v.VisitorId == visitorId);

        //    if (log == null)
        //    {
        //        db.VisitorLogs.Add(new VisitorLog
        //        {
        //            VisitorId = visitorId,
        //            SessionId = sessionId,
        //            VisitTime = DateTime.Now,
        //            LastActiveTime = DateTime.Now,
        //            // test sau sẽ dung IP thực tế
        //            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
        //            UserAgent = context.Request.Headers["User-Agent"].ToString()
        //        });
        //    }
        //    else
        //    {

        //        _logger.LogInformation("Existing visitor: " + visitorId);
        //        log.LastActiveTime = DateTime.Now;
        //        log.SessionId = sessionId;
        //    }

        //    await db.SaveChangesAsync();
        //    await _next(context);
        //}
    }

}
