namespace Chat_Website.Models.DataBase
{
    public class ChatRoom
    {
        public int Id { get; set; }

        public string? Title { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<UserApplication> Users { get; set; } = new List<UserApplication>();
    }
}