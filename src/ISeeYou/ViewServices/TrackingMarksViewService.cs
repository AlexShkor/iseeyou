using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;

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
    }
}