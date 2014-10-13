using Newtonsoft.Json;

namespace ISeeYou.Vk.Dto
{
    [JsonObject]
    public class VkUser
    {
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("photo_200")]
        public string AvatarUrl { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }
    }
}
