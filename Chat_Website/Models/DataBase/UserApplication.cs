namespace Chat_Website.Models.DataBase
{
    public class UserApplication : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }
        public bool IsOnline { get; set; }
        public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}