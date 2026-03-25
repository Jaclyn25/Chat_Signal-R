using Microsoft.AspNetCore.Authorization;

namespace Chat_Website.Controllers.Chat
{
    [Authorize]
    public class ChatRoomController(IRepostriy<ChatRoom> roomRepo, IRepostriy<ChatMessage> messageRepo, UserManager<UserApplication> userManager) : Controller
    {
        private readonly IRepostriy<ChatRoom> _roomRepo = roomRepo;
        private readonly IRepostriy<ChatMessage> _messageRepo = messageRepo;
        private readonly UserManager<UserApplication> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            // Fetch all users except current user
            var allUsers = _userManager.Users
                .Where(u => u.Id != currentUser.Id)
                .ToList();

            return View(allUsers);
        }

        [HttpGet]
        public IActionResult Chat(int id)
        {
            var room = _roomRepo.GetByID(id);
            if (room == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);

            var viewModel = new ChatRoomViewModel
            {
                RoomId = room.Id,
                RoomTitle = room.Title,
                IsPrivate = room.Title.Contains('_')
            };
            if (viewModel.IsPrivate)
            {
                var usersInRoom = room.Title.Split('_');
                var otherUserId = usersInRoom.FirstOrDefault(uId => uId != currentUserId);
                var otherUser = !string.IsNullOrEmpty(otherUserId) ? _userManager.FindByIdAsync(otherUserId).Result : null;
                viewModel.RoomTitle = otherUser?.UserName ?? "Private Chat";
            }
            viewModel.Messages = [
                .._messageRepo.GetAll()
                    .Where(m => m.ChatRoomId == id)
                    .Include(m => m.UserApplication)
                    .Where(m => m.UserApplication != null)
                    .Select(m => new ChatMessageViewModel
                    {
                        Id = m.Id,
                        UserName = m.UserApplication!.UserName ?? "Unknown",
                        Content = m.Content,
                        Timestamp = m.Timestamp,
                        UserId = m.UserApplicationId,
                        IsSeen = m.IsSeen,
                        ProfileImageUrl = m.UserApplication!.ProfileImageUrl
                    })
            ];

            return View(viewModel);
        }
        public IActionResult PrivateChat(string receiverId)
        {
            var senderId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(senderId))
                return Unauthorized();

            string roomName = string.Compare(senderId, receiverId) < 0
                              ? $"{senderId}_{receiverId}"
                              : $"{receiverId}_{senderId}";

            var room = _roomRepo.GetAll().FirstOrDefault(r => r.Title == roomName);

            if (room == null)
            {
                room = new ChatRoom
                {
                    Title = roomName,
                    CreatedAt = DateTime.Now
                };
                _roomRepo.Add(room);
                _roomRepo.Save();
            }
            return RedirectToAction("Chat", new { id = room.Id });
        }
    }
}
