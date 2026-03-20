namespace ManageCars.Middleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
            if (context.Request.Path.StartsWithSegments("/chatHub"))
            {
                await _next(context);
                return;
            }
            if (path.StartsWith("/admin"))
            {

                if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole("Admin"))
                {
                    context.Response.Redirect("/Home/Error");
                    return;
                }


            }


            //if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            //{
            //    if (path.StartsWith("/home/login") || path.StartsWith("/home/register"))
            //    {
            //        context.Response.Redirect("/Home/Index");
            //        return;
            //    }


            //}

            await _next(context);
        }
    }
}
