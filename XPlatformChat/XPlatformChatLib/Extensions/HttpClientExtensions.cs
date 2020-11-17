using System.Net.Http;
using System.Net.Http.Headers;

namespace XPlatformChat.Lib.Extensions
{
    public static class HttpClientExtensions
    {
        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
