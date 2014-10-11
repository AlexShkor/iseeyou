using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class EventsViewService : ViewService<EventView>
    {
        public EventsViewService(MongoViewDatabase database) : base(database)
        {
        }

        public override MongoCollection<EventView> Items
        {
            get { return Database.Events; }
        }
    }

    public class AppsViewService : ViewService<AppView>
    {
        public AppsViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        public override MongoCollection<AppView> Items
        {
            get { return Database.Apps; }
        }
    }
}