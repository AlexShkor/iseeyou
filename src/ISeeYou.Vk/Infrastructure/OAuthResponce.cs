using Newtonsoft.Json;

namespace ISeeYou.Vk.Infrastructure
{
    [JsonObject]
    public class OAuthResponce
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
