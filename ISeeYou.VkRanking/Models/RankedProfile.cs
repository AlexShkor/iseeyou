using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISeeYou.VkRanking.Models
{
    public class RankedProfile
    {
        public RankedProfile(long id, int startRank)
        {
            Id = id;
            Rank = startRank;
        }

        public int Rank { get; set; }
        public long Id { get; set; }
    }
}
