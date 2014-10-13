namespace ISeeYou
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
