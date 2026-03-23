namespace Chat_Website.Models.DataBase
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public int ChatRoomId { get; set; }
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public string UserApplicationId { get; set; } = string.Empty;
        public virtual UserApplication UserApplication { get; set; } = null!;
        public bool IsSeen { get; set; } = false;
    }
}