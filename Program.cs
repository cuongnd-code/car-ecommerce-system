using System.Net.WebSockets;
using ManageCars.Hubs;
using ManageCars.Middleware;
using ManageCars.Models; // Goi namespace chua lop DbContext (AppDbContext) cua ban
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore; // Dung de cau hinh Entity Framework Core

// Tao doi tuong builder - day la buoc dau tien khoi tao ung dung web (chua config, DI container, logging...)
var builder = WebApplication.CreateBuilder(args);
var orderWebSocketHandler = new ManageCars.Middleware.OrderWebSocketHandler();



builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/home/login"; // Duong dan toi trang dang nhap
        options.AccessDeniedPath = "/Login/AccessDenied"; // Duong dan toi trang khong duoc phep truy cap
        options.Cookie.HttpOnly = true; // Chi cho phep trinh duyet truy cap cookie, khong cho Javascript truy cap
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


builder.Services.AddAuthorization(options =>
{
    // Thiet lap chinh quyen cho cac yeu cau
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});
//builder.Services.AddLogging(loggingBuilder =>
//{
//	// Dang ky cac dich vu log: Console, Debug, Trace
//	loggingBuilder.AddConsole();
//	loggingBuilder.AddDebug();
//	loggingBuilder.AddTraceSource("Information, ActivityTracing");
//});
// Dang ky DbContext de dung MySQL (doc chuoi ket noi tu appsettings.json)
// Dieu nay cho phep ban truy cap du lieu bang Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Doc chuoi ket noi tu file cau hinh
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) // Tu dong phat hien phien ban MySQL
    ));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
// Dang ky dich vu MVC: cho phep dung Controller + View (co giao dien Razor .cshtml)
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // KHONG HOAT DONG THI -> offline
});

builder.Services.AddSignalR();



// Tao ung dung tu cau hinh da thiet lap
var app = builder.Build();

// Neu moi truong KHONG phai Development (vi du: Production)


if (!app.Environment.IsDevelopment())
{
    // Khi co loi, chuyen huong den trang /Home/Error
    //app.UseExceptionHandler("/Home/Error");

    // HSTS: tang bao mat khi chay HTTPS (bat trinh duyet dung HTTPS trong 30 ngay)
    //app.UseHsts();
}

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

app.UseMiddleware<ManageCars.Middleware.ExceptionMiddleware>();
// Tu dong chuyen moi yeu cau HTTP sang HTTPS
app.UseHttpsRedirection();

// Cho phep truy cap cac file tinh trong wwwroot (CSS, JS, hinh anh...)
app.UseStaticFiles();



// Bat dau he thong dinh tuyen (xu ly URL -> Controller -> View)
app.UseRouting();

//kich hoach kiem tra
app.UseAuthentication();

// Kich hoat phan quyen nguoi dung (neu dung chuc nang login)
app.UseAuthorization();

app.UseWebSockets();


app.MapHub<CarHub>("/carHub");

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


app.UseSession();

app.UseMiddleware<VisitorTrackingMiddleware>();
// Thiet lap tuyen mac dinh: URL / se chay HomeController, action Index
// Vi du: /Car/List -> goi CarController.List()
//        / -> goi HomeController.Index()
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Chay ung dung - mo cong va bat dau lang nghe request
app.Run();


