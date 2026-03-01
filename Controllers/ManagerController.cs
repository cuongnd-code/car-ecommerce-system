using ManageCars.Models;
using ManageCars.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ManagerController> _logger;

        public ManagerController(ILogger<ManagerController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpPost()]
        public async Task<IActionResult> SendEmail([FromForm] EmailRequest emailRequest, [FromServices] IConfiguration configuration)
        {
            try
            {


                _logger.LogInformation("check mail request" + emailRequest.To);
                _logger.LogInformation("check mail request" + emailRequest.Subject);

                var emailSettings = configuration.GetSection("EmailSettings");

                // Lấy ra giá trị
                if (emailSettings == null)
                {
                    throw new Exception("Email settings not configured properly.");
                }
                if (string.IsNullOrEmpty(emailSettings["SmtpServer"]) ||
                   string.IsNullOrEmpty(emailSettings["Port"]) ||
                   string.IsNullOrEmpty(emailSettings["SenderEmail"]) ||
                   string.IsNullOrEmpty(emailSettings["Password"]) ||
                   string.IsNullOrEmpty(emailSettings["SenderName"]))
                {
                    throw new Exception("Email settings are incomplete.");
                }

                string smtpServer = emailSettings["SmtpSe rver"] ?? "smtp.gmail.com";
                int port = int.Parse(emailSettings["Port"] ?? "587");
                string senderEmail = emailSettings["SenderEmail"] ?? throw new Exception("SenderEmail is missing");
                string password = emailSettings["Password"] ?? throw new Exception("Password is missing");
                using (var client = new System.Net.Mail.SmtpClient(smtpServer, port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(senderEmail, password);
                    var mailMessage = new System.Net.Mail.MailMessage
                    {
                        From = new System.Net.Mail.MailAddress(senderEmail, emailSettings["SenderName"]),
                        Subject = emailRequest.Subject,
                        Body = emailRequest.Body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(emailRequest.To);
                    await client.SendMailAsync(mailMessage);

                };


                return Json(new { Success = true, Message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return Json(new { Success = false, Message = "Error sending email." });
            }

        }



    }
}
