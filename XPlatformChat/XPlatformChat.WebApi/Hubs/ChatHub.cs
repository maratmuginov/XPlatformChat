using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using XPlatformChat.Lib.Models;
using XPlatformChat.WebApi.Data;

namespace XPlatformChat.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;
        private readonly UserManager<ChatUser> _userManager;
        public ChatHub(ChatDbContext context, UserManager<ChatUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(Message message)
        {
            var sender = await _userManager.FindByNameAsync(message.Username);
            message.UserId = sender.Id;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
