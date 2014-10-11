using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISeeYou.VkRanking.Models
{
    public class RankedProfile
    {
        public RankedProfile(long id, int startRank, long sourceId)
        {
            Id = id;
            Rank = startRank;
            SourceId = sourceId;
        }

        public int Rank { get; set; }
        public long Id { get; set; }
        public long SourceId { get; set; }
    }
}
