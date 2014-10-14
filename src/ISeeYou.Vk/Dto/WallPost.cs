using System.Collections.Generic;

namespace ISeeYou.Vk.Api
{
    public class WallPost
    {
        public long id { get; set; }
        public long date { get; set; }
        public long owner_id { get; set; }
        public string text { get; set; }
        public List<AttachmetVk> attachments { get; set; }
        public LikesInfo likes { get; set; }

    }
}