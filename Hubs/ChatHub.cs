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
                string displayName;
                bool isAdmin = Context.User.IsInRole("Admin");

                var httpContext = Context.GetHttpContext();

                if (isAdmin)
                {
                    senderKey = "admin_manager";
                    displayName = "Quản trị viên";
                }
                else if (Context.User.Identity?.IsAuthenticated == true)
                {
                    var idClaim = Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                    senderKey = "user_" + (idClaim?.Value ?? "unknown");
                    displayName = $"Khách hàng {senderKey.Substring(Math.Max(0, senderKey.Length - 4))}";
                }
                else
                {
                    senderKey = httpContext.Request.Cookies["GuestId"] ?? $"guest_{Context.ConnectionId}";
                    displayName = "Khách vãng lai";
                }

                string createdAt = "[" + DateTime.Now.ToString("HH:mm") + "]";

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
                    await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
                }

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


                await Clients.Group(conversationId.ToString())
                             .SendAsync("ReceiveMessage", displayName, message, createdAt);


                if (!isAdmin)
                {
                    var botResponse = _chatBoxService.Prompt(message);
                    await Clients.Group(conversationId.ToString())
                                 .SendAsync("ReceiveChatMessage", botResponse);
                }

                return conversationId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Lỗi: {ex.Message}");
                throw new HubException("Không thể gửi tin nhắn.");
            }
        }


        public async Task AdminJoinChat(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
            _logger.LogInformation($"Admin {Context.ConnectionId} đã tham gia phòng: {conversationId}");
        }


    }
}
