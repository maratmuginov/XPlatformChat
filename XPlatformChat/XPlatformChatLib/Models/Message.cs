using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XPlatformChat.Lib.Models
{
    public class Message
    {
        [Key]
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Sent { get; set; }
        public string Username { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        /*
         * Serialization ignored because we don't want to send the client data about users,
         * while retaining our easy Domain access through EF Core.
         */
        [JsonIgnore]
        public virtual ChatUser User { get; set; }
    }
}
