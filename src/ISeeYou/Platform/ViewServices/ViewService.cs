using System.Collections.Generic;
using ISeeYou.Databases;
using ISeeYou.Platform.Mongo;
using ISeeYou.Views;
using MongoDB.Driver;

namespace ISeeYou.Platform.ViewServices
{
    public abstract class ViewService<T>
    {
        protected readonly MongoViewDatabase Database;

        protected ViewService(MongoViewDatabase database)
        {
            Database = database;
        }

        public abstract MongoCollection<T> Items { get; }

        public virtual T GetById(string id)
        {
            return Items.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items.FindAll();
        }

        public void Save(T user)
        {
            Items.Save(user);
        }
    }
}
