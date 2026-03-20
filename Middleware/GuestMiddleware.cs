namespace ManageCars.Middleware
{
    public class GuestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GuestMiddleware> _logger;


        public GuestMiddleware(RequestDelegate next, ILogger<GuestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }



        public async Task InvokeAsync(HttpContext context)
        {


            if (context.Request.Path.StartsWithSegments("/chatHub"))
            {
                await _next(context);
                return;
            }
            // Người dùng ĐÃ đăng nhập
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                // Nếu vẫn còn sót Cookie GuestId thì xóa nó đi cho sạch
                if (context.Request.Cookies.ContainsKey("GuestId"))
                {
                    context.Response.Cookies.Delete("GuestId");
                }
            }
            //  Người dùng CHƯA đăng nhập 
            else
            {
                if (!context.Request.Cookies.ContainsKey("GuestId"))
                {
                    var guestId = "guest_" + Guid.NewGuid().ToString("N");
                    _logger.LogInformation(guestId);
                    context.Response.Cookies.Append("GuestId", guestId, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(7),
                        HttpOnly = true,
                        IsEssential = true
                    });
                }
            }




            await _next(context);
        }
    }
}
