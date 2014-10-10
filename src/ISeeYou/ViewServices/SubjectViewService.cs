using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class SubjectViewService: ViewService<SubjectView>
    {
        public SubjectViewService(MongoViewDatabase database) : base(database)
        {
        }

        public override MongoCollection<SubjectView> Items
        {
            get { return Database.Subjects; }
        }
    }
}