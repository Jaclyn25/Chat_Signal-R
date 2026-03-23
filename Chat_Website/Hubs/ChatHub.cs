namespace Chat_Website.Hubs
{
    public class ChatHub(IRepostriy<ChatMessage> messageRepo, UserManager<UserApplication> userManager) : Hub
    {
        private readonly IRepostriy<ChatMessage> _messageRepo = messageRepo;
        private readonly UserManager<UserApplication> _userManager = userManager;
        public async Task SendMessage(string message, string roomId)
        {
            if (Context.User?.Identity == null)
                return;

            var userId = _userManager.GetUserId(Context.User);
            var userName = Context.User.Identity.Name;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                return;
            var chatMsg = new ChatMessage
            {
                Content = message,
                ChatRoomId = int.Parse(roomId),
                UserApplicationId = userId,
                Timestamp = DateTime.Now
            };

            _messageRepo.Add(chatMsg);
            _messageRepo.Save();
            await Clients.Group(roomId).SendAsync("ReceiveMessage", userName, message, DateTime.Now.ToString("HH:mm"), chatMsg.Id);
        }

        public async Task MarkAsSeen(int messageId, string roomId)
        {
            var message = _messageRepo.GetByID(messageId);
            if (message != null && !message.IsSeen)
            {
                message.IsSeen = true;
                _messageRepo.Update(message);
                _messageRepo.Save();
                await Clients.Group(roomId).SendAsync("MessageSeen", messageId);
            }
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendTyping(string roomId)
        {
            var userName = Context.User?.Identity?.Name;
            await Clients.OthersInGroup(roomId).SendAsync("UserTyping", userName);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = _userManager.GetUserId(Context.User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsOnline = true;
                    await _userManager.UpdateAsync(user);
                    await Clients.All.SendAsync("UserStatusUpdate", userId, true);
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _userManager.GetUserId(Context.User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsOnline = false;
                    await _userManager.UpdateAsync(user);
                    await Clients.All.SendAsync("UserStatusUpdate", userId, false);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
