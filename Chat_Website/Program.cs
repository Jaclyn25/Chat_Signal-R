using Chat_Website.Hubs;
using Chat_Website.Repostries.Impelemtation;

namespace Chat_Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Chat")));
            builder.Services.AddIdentity<UserApplication, IdentityRole>(options => {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ChatDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true; 
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddSignalR();
            // إضافة الـ Generic Repository للـ Services
            builder.Services.AddScoped(typeof(IRepostriy<ChatRoom>), typeof(GenericIRepostriy<ChatRoom>));
            builder.Services.AddScoped(typeof(IRepostriy<ChatMessage>), typeof(GenericIRepostriy<ChatMessage>));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();
            
            app.UseSession();
            app.MapHub<ChatHub>("/chatHub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
