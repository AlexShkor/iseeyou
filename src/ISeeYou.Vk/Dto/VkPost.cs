using Newtonsoft.Json;

namespace ISeeYou.Vk.Dto
{
    [JsonObject]
    public class VkPost
    {
        [JsonProperty("post_id")]
        public string PostId { get; set; }
    }
}
