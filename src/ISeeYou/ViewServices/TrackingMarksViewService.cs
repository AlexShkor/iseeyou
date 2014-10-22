using System;
using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ISeeYou.ViewServices
{
    public class TrackingMarksViewService : ViewService<TrackingMark>
    {
        public TrackingMarksViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        public override MongoCollection<TrackingMark> Items
        {
            get { return Database.TrackingMarks; }
        }

        public DateTime? GetSourceMark(int userId)
        {
            var mark = Items.FindOneById(BuildSourceMarkId(userId));
            return mark != null ? mark.Tracked : (DateTime?)null;
        }

        public void SaveSourceMark(int userId, DateTime traked)
        {
            Items.Update(Query<TrackingMark>.EQ(x => x.Id, BuildSourceMarkId(userId)),
                Update<TrackingMark>.Set(x => x.Tracked, traked), UpdateFlags.Upsert);
        }

        private string BuildSourceMarkId(int userId)
        {
            return "source" + userId;
        }
    }
}