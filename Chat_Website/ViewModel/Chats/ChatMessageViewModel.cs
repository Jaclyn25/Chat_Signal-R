namespace Chat_Website.ViewModel.Chats
{
    public class ChatMessageViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserId { get; set; }
        public bool IsSeen { get; set; }
    }
}
