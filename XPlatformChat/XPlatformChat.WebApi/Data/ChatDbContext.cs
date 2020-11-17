using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XPlatformChat.Lib.Models;

namespace XPlatformChat.WebApi.Data
{
    public class ChatDbContext : IdentityDbContext<ChatUser>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : 
            base(options) {
            
        }

        public DbSet<Message> Messages { get; set; }
    }
}
