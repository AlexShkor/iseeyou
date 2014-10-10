using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.Platform.ViewServices;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class SourcesViewService: ViewService<SourceDocument>
    {
        public SourcesViewService(MongoViewDatabase database) : base(database)
        {
        }

        public override MongoCollection<SourceDocument> Items
        {
            get { return Database.Sources; }
        }
    }
}