namespace ManageCars.Helper
{
    public class VisitorHelper
    {
        public static string GetOrCreateVisitorId(HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("VisitorId", out string visitorId))
            {
                visitorId = Guid.NewGuid().ToString();

                context.Response.Cookies.Append("VisitorId", visitorId, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1),
                    HttpOnly = true,
                    IsEssential = true
                });
            }
            return visitorId;
        }
    }
}
