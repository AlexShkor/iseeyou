using System;
using ISeeYou.Documents;
using ISeeYou.Platform.Mongo;
using ISeeYou.Views;
using MongoDB.Driver;

namespace ISeeYou.Databases
{
    public class MongoViewDatabase
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        public MongoUrl MongoUrl { get; private set; }

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoViewDatabase(String connectionString)
        {
            MongoUrl = MongoUrl.Create(connectionString);
            _databaseName = MongoUrl.DatabaseName;
            _server = MongoServer.Create(connectionString);
        }

        public MongoViewDatabase EnsureIndexes()
        {
            // build indexes here
            return this;
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _server.GetDatabase(_databaseName); }
        }

        public MongoCollection GetCollection(String collectionName)
        {
            return Database.GetCollection(collectionName);
        }

        protected MongoCollection<TDocument> GetCollection<TDocument>(String collectionName)
        {
            return Database.GetCollection<TDocument>(collectionName);
        }


        public MongoCollection<UserView> Users
        {
            get { return GetCollection<UserView>(ViewCollections.Users); }
        }

        public MongoCollection<SiteView> Sites
        {
            get { return GetCollection<SiteView>(ViewCollections.Sites); }
        }

        public MongoCollection<SourceDocument> Sources
        {
            get { return GetCollection<SourceDocument>(ViewCollections.Sources); }
        }

        public MongoCollection<SubjectView> Subjects
        {
            get { return GetCollection<SubjectView>(ViewCollections.Subjects); }
        }

        public MongoCollection<EventView> Events
        {
            get { return GetCollection<EventView>(ViewCollections.Events); }
        }

        public MongoCollection<AppView> Apps
        {
            get { return GetCollection<AppView>(ViewCollections.Apps); }
        }

    }
}
