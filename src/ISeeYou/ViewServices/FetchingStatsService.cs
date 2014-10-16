using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class FetchingStatsService: ViewService<FetchingStats>
    {
        public FetchingStatsService(MongoViewDatabase database)
            : base(database)
        {
        }

        public override MongoCollection<FetchingStats> Items
        {
            get { return Database.FetchingStats; }
        }
    }
}