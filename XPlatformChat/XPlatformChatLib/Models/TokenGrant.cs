using System;
using System.Text.Json.Serialization;

namespace XPlatformChat.Lib.Models
{
    public class TokenGrant
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
        [JsonPropertyName("userId")]
        public string Username { get; set; }
    }
}
