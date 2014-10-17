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

        public void Insert(EventView eventView)
        {
            Items.Insert(eventView, new WriteConcern()
            {
                FSync = false,
            });
        }
    }
}