using ManageCars.Controllers.Service;
using ManageCars.Models;
using Microsoft.AspNetCore.SignalR;

namespace ManageCars.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ChatHub> _logger;

        public readonly ChatBoxService _chatBoxService;

        public ChatHub(AppDbContext context, ILogger<ChatHub> logger, ChatBoxService chatBoxService)
        {
            _context = context;
            _logger = logger;
            _chatBoxService = chatBoxService;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Kết nối mới: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
        public async Task JoinConversation(int conversationId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                conversationId.ToString()
            );

            _logger.LogInformation($"Join conversation {conversationId}");
        }


        public async Task<int> SendMessage(int conversationId, string message)
        {
            try
            {
                _logger.LogInformation("--- SendMessage Start ---");
                string senderKey;
                var httpContext = Context.GetHttpContext();

                // 1. Xác định Người gửi (User hoặc Guest)
                if (Context.User.Identity?.IsAuthenticated == true)
                {
                    var idClaim = Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                    senderKey = "user_" + (idClaim?.Value ?? "unknown");
                }
                else
                {
                    senderKey = httpContext.Request.Cookies["GuestId"];

                    if (string.IsNullOrEmpty(senderKey))
                    {
                        senderKey = "guest_" + Context.ConnectionId;
                    }
                }


                string displayName = senderKey.StartsWith("user_") ? $"User{senderKey.Substring(33)}" : $"Guest {senderKey.Substring(33)}";

                string createdAt = "[" + DateTime.UtcNow.ToString("HH:mm") + "]";

                // 2. Xử lý Conversation
                if (conversationId == 0)
                {
                    var newConversation = new Conversation
                    {
                        SenderKey = senderKey,
                        CreatedAt = DateTime.UtcNow,
                        DisplayName = displayName,
                        IsClosed = false
                    };
                    _context.Conversations.Add(newConversation);
                    await _context.SaveChangesAsync();
                    conversationId = newConversation.Id;

                    // Tự động cho User vào Group của chính mình
                    await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
                    _logger.LogInformation($"Created New Conv ID: {conversationId}");
                }

                // 3. Lưu Tin nhắn vào Database
                var newMessage = new Message
                {
                    ConversationId = conversationId,

                    SenderKey = senderKey,
                    Content = message,
                    DisplayName = displayName,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();



                // 4. Bắn tin nhắn tới Group (Để cả Admin và User đều nhận được)
                // Lưu ý: conversationId.Value.ToString() phải khớp với tên Group lúc Join
                await Clients.Group(conversationId.ToString())
                             .SendAsync("ReceiveMessage", displayName, message, createdAt);



                await Clients.Group(conversationId.ToString())
                            .SendAsync("ReceiveChatMessage", _chatBoxService.Prompt(message));

                return conversationId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Lỗi nghiêm trọng: {ex.Message}");
                // HubException sẽ gửi thông báo lỗi an toàn về phía Client
                throw new HubException("Không thể gửi tin nhắn. Vui lòng thử lại!");
            }
        }


    }
}
