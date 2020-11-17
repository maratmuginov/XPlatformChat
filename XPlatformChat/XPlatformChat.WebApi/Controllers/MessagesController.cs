using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPlatformChat.Lib.Models;
using XPlatformChat.WebApi.Data;

namespace XPlatformChat.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ChatDbContext _context;

        public MessagesController(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }
    }
}
