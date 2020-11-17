using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace XPlatformChat.Lib.Models
{
    public class ChatUser : IdentityUser
    {
        public ChatUser()
        {
            Messages = new HashSet<Message>();
        }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
