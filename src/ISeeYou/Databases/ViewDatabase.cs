using System;
using ISeeYou.Views;
using Uniform;

namespace ISeeYou.Databases
{
    public static class ViewDatabases
    {
        public const string Mongodb = "mongodb";
        public const string Sql = "sql";
    }

    public static class ViewCollections
    {
        public const string Users = "users";
        public const string Sites = "sites";
        public const string Tables = "tables";
        public const string Sources = "sources";
        public const string Subjects = "subjects";
        public const string Events = "events";
        public const string Apps = "apps";
        public const string TrackingMarks = "tracking_marks";
        public const string FetchingStats = "fetching_stats";
    }

    public class ViewDatabase
    {
        #region ViewDatabase
        private readonly UniformDatabase _db;

        public ViewDatabase(UniformDatabase db)
        {
            _db = db;
        }

        /// <summary>
        /// Helper method for mongodb collections
        /// </summary>
        private IDocumentCollection<TDocument> GetMongoCollection<TDocument>(String collectionName) where TDocument : new()
        {
            return _db.GetCollection<TDocument>(ViewDatabases.Mongodb, collectionName);
        }

        /// <summary>
        /// Helper method for sql collections (tables)
        /// </summary>
        private IDocumentCollection<TDocument> GetSqlCollection<TDocument>(String tableName) where TDocument : new()
        {
            return _db.GetCollection<TDocument>(ViewDatabases.Sql, tableName);
        }

        #endregion

        

        public IDocumentCollection<UserView> Users
        {
            get { return GetMongoCollection<UserView>(ViewCollections.Users); }
        }

        public IDocumentCollection<SiteView> Sites
        {
            get { return GetMongoCollection<SiteView>(ViewCollections.Sites); }
        }

    }
}