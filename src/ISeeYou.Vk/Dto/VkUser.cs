using System.Collections.Generic;
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

        [JsonProperty("photo_100")]
        public string Avatar100px{ get; set; }

        [JsonProperty("sex")]
        public Sex Sex { get; set; }

        [JsonProperty("bdate")]
        public string bdate { get; set; }

        [JsonProperty("lists")]
        public string lists { get; set; }

        [JsonProperty("common_count")]
        public int common_count { get; set; }

        [JsonProperty("followers_count")]
        public int followers_count { get; set; }

        public int hidden { get; set; }

        public Education education { get; set; }
        public List<School> schools { get; set; }

        public List<Relative> Relatives { get; set; }

    }

    public class Education
    {
        public string university { get; set; }
        public string university_name { get; set; }
        public string faculty { get; set; }
        public string faculty_name { get; set; }
        public string graduation { get; set; }
    }

    public class School
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Relative
    {
        [JsonProperty("uid")]
        public int Id { get; set; }

        public string type { get; set; }
    }

    public enum Sex
    {
        None = 0,
        Female = 1,
        Male = 2
    }
}
