using System.Net.WebSockets;
using ManageCars.Controllers.Service;
using ManageCars.Hubs;
using ManageCars.Middleware;
using ManageCars.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var orderWebSocketHandler = new OrderWebSocketHandler();

// Đăng ký các dịch vụ nghiệp vụ (Dependency Injection)
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<ChatBoxService>();
builder.Services.AddScoped<MeetingService>();

// Cấu hình Cookie Authentication
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/home/login";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                if (ctx.Request.Path.StartsWithSegments("/shop") &&
                    ctx.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                ctx.Response.Redirect(ctx.RedirectUri);
                return Task.CompletedTask;
            }
        };
    });

// Cấu hình phân quyền theo vai trò (Role-Based Authorization)
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
        options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
    });

// Đăng ký DbContext kết nối MySQL qua Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();
app.UseSession();

// Đăng ký chuỗi Custom Middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<GuestMiddleware>();
app.UseMiddleware<LoginMiddleware>();

// Định tuyến SignalR Hubs
app.MapHub<CarHub>("/carHub");
app.MapHub<ChatHub>("/chatHub");

// Xử lý kết nối Native WebSocket cho trạng thái đơn hàng
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
    {
        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await orderWebSocketHandler.HandleWebSocketConnection(webSocket, context);
    }
    else
    {
        await next();
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
