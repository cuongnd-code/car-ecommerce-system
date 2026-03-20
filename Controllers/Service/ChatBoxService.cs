using System.Text.RegularExpressions;
using ManageCars.Helper;
using ManageCars.Models;
namespace ManageCars.Controllers.Service
{




    public class ChatBoxService
    {
        private readonly ILogger<ChatBoxService> _logger;
        private readonly AppDbContext _context;

        public ChatBoxService(ILogger<ChatBoxService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        public void CreateConversation()
        {

        }

        public string Prompt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Anh/chị vui lòng nhập nội dung cần tư vấn.";

            // =============================
            // 1️⃣ Normalize input
            // =============================
            input = input.ToLower();
            input = TextHelper.RemoveVietnameseDiacritics(input);
            input = Regex.Replace(input, @"[^\w\s]", "");
            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim();

            // =============================
            // 2️⃣ SỐ CHỖ
            // =============================
            var seatMatch = Regex.Match(input, @"(\d+)\s*cho\b");

            if (seatMatch.Success)
            {
                var seats = seatMatch.Groups[1].Value;
                return $"Tôi đã mở danh sách xe {seats} chỗ cho anh/chị. Anh/chị vui lòng tham khảo tại trang tìm kiếm.";
            }

            // =============================
            // 3️⃣ GIÁ
            // =============================
            if (input.Contains("gia") || input.Contains("bao nhieu tien"))
            {
                return "Anh/chị vui lòng chọn mẫu xe cụ thể để xem giá chi tiết. Hoặc cho tôi biết ngân sách dự kiến để tôi gợi ý phù hợp.";
            }

            // =============================
            // 4️⃣ TRẢ GÓP
            // =============================
            if (input.Contains("tra gop") || input.Contains("vay"))
            {
                return "Chúng tôi hỗ trợ trả góp lên đến 70-80% giá trị xe. Thủ tục gồm CMND/CCCD, hộ khẩu và chứng minh thu nhập. Anh/chị muốn tôi tư vấn chi tiết cho mẫu xe nào?";
            }

            // =============================
            // 5️⃣ BẢO HÀNH
            // =============================
            if (input.Contains("bao hanh"))
            {
                return "Các dòng xe được bảo hành chính hãng từ 3-5 năm hoặc 100.000-150.000km tùy mẫu. Anh/chị quan tâm đến dòng xe nào để tôi cung cấp thông tin chính xác hơn?";
            }

            // =============================
            // 6️⃣ SO SÁNH SUV VS SEDAN
            // =============================
            if (input.Contains("suv") && input.Contains("sedan"))
            {
                return @"SUV có gầm cao, không gian rộng và phù hợp gia đình hoặc đi xa. 
Sedan tiết kiệm nhiên liệu, dễ lái trong đô thị và giá thường mềm hơn. 
Anh/chị ưu tiên không gian rộng hay tiết kiệm chi phí?";
            }

            // =============================
            // 7️⃣ KHÔNG HIỂU CÂU HỎI
            // =============================
            return "Anh/chị có thể cho tôi biết nhu cầu cụ thể hơn (số chỗ, ngân sách, loại xe) để tôi hỗ trợ tốt nhất không? Hoặc Anh Chị hãy liên hệ trực tiếp với chúng tôi";
        }


    }
}
