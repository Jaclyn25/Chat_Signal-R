namespace Chat_Website.Data.ContextDB
{
    public class ChatDbContext:IdentityDbContext<UserApplication>
    {
        public DbSet<ChatRoom> chatrooms { get; set; }
        public DbSet<ChatMessage> messages { get; set; }
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatRoom>()
                .HasMany(r => r.Users)
                .WithMany(u => u.ChatRooms)
                .UsingEntity(j => j.ToTable("ChatRoomUserApplication"));
        }
    }
}
