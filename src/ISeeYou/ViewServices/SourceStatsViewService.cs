using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.Platform.ViewServices;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class SourceStatsViewService: ViewService<SourceStats>
    {
        public SourceStatsViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        public override MongoCollection<SourceStats> Items
        {
            get { return Database.SourceStats; }
        }

    }
}