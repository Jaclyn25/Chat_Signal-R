namespace Chat_Website.ViewModel.Chats
{
    public class ChatRoomViewModel
    {
        public int RoomId { get; set; }
        public string? RoomTitle { get; set; }
        public bool IsPrivate { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
        public ICollection<ChatMessageViewModel> Messages { get; set; } = [];
        public string? NewMessageContent { get; set; }
    }
}
