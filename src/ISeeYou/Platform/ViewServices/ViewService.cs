using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ISeeYou.Databases;
using ISeeYou.Platform.Mongo;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

        public void Set<TProperty>(object id, Expression<Func<T, TProperty>> expr, TProperty value)
        {
            Items.Update(Query.EQ("_id", BsonValue.Create(id)),
                       Update<T>.Set(expr, value));
        }



        public void Inc(object id, Expression<Func<T, long>> expr, long value)
        {
            Items.Update(Query.EQ("_id", BsonValue.Create(id)),
                Update<T>.Inc(expr, value));
        }

        public void InsertAsync(T doc)
        {
            try
            {

                Items.Insert(doc,
                    new WriteConcern()
                    {
                        FSync = false,
                        Journal = false,
                    });
            }
            catch (WriteConcernException  e)
            {
            }
        }
    }
}
