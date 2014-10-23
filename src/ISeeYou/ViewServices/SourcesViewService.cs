using System;
using System.Collections.Generic;
using System.Linq;
using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.Platform.ViewServices;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ISeeYou.ViewServices
{
    public class SourcesViewService : ViewService<SourceDocument>
    {
        public SourcesViewService(MongoViewDatabase database) : base(database)
        {
        }

        public override MongoCollection<SourceDocument> Items
        {
            get { return Database.Sources; }
        }

        public IEnumerable<int> GetSourceIdsFor(int subjectId)
        {
            return Items.Find(Query<SourceDocument>.EQ(x => x.SubjectId, subjectId)).SetFields("SourceId").Select(x => x.SourceId);
        }

        public long GetSourcesCount(int subjectId, DateTime addedAfter)
        {
            return Items.Find(Query.And(
                Query<SourceDocument>.EQ(x => x.SubjectId, subjectId),
                Query<SourceDocument>.EQ(x => x.New, true),
                    Query<SourceDocument>.GT(x => x.Added, addedAfter))).Count();
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

        public long GetPhotosCount(int sourceId, DateTime addedAfter)
        {
            return Items.Find(Query.And(
                Query<PhotoDocument>.EQ(x => x.SourceId, sourceId),
                Query<PhotoDocument>.EQ(x => x.New, true),
                    Query<PhotoDocument>.GT(x => x.Added, addedAfter))).Count();
        }

        public IEnumerable<int> GetPhotoIdsFor(int userId)
        {
            return Items.Find(Query<PhotoDocument>.EQ(x => x.SourceId, userId)).SetFields("PhotoId").Select(x => x.PhotoId);
        }
    }

    public class PhotoDocument
    {
        [BsonId]
        public string Id { get; set; }

        public DateTime NextFetching { get; set; }
        public int SourceId { get; set; }
        public int PhotoId { get; set; }

        public DateTime Created { get; set; }
        public string Image { get; set; }
        public string ImageBig { get; set; }
        public DateTime? FetchingEnd { get; set; }
        public bool New { get; set; }
        public DateTime Added { get; set; }
        public DateTime? FetchingStarted { get; set; }
        public DateTime? FetchedFirstTime { get; set; }
        public int Likes { get; set; }
    }
}