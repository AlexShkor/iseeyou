using System;
using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.Platform.ViewServices;
using MongoDB.Bson.Serialization.Attributes;
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
    public class PhotoDocumentsService: ViewService<PhotoDocument>
    {
        public PhotoDocumentsService(MongoViewDatabase database)
            : base(database)
        {
        }

        public override MongoCollection<PhotoDocument> Items
        {
            get { return Database.PhotoDocuments; }
        }
    }

    public class PhotoDocument
    {
        [BsonId]
        public string Id { get; set; }

        public DateTime NextFetching { get; set; }

        public int Rank { get; set; }
        public DateTime Created { get; set; }
        public string Image { get; set; }
        public string ImageBig { get; set; }
        public DateTime? FetchingEnd { get; set; }
    }
}