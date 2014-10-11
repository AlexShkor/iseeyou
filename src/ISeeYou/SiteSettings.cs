using ISeeYou.Platform.Settings;

namespace ISeeYou
{
    public class SiteSettings
    {
        public const string SiteId = "ISeeYou";

        //[SettingsProperty("mongo.events_connection_string")]
        //public string MongoEventsConnectionString { get; set; }

        [SettingsProperty("mongo.views_connection_string")]
        public string MongoViewConnectionString { get; set; }
            
        //[SettingsProperty("mongo.logs_connection_string")]
        //public string MongoLogsConnectionString { get; set; }

        //[SettingsProperty("mongo.admin_connection_string")]
        //public string MongoAdminConnectionString { get; set; }

        [SettingsProperty("sendgrid.username")]
        public string SendgridUsername { get; set; }

        [SettingsProperty("sendgrid.password")]
        public string SendgridPassword { get; set; }

        [SettingsProperty("application.base-url")]
        public string AppBaseUrl { get; set; }

        [SettingsProperty("application.environment")]
        public string AppEnvironment { get; set; }

        [SettingsProperty("488312727900241")]
        public string FacebookAppId { get; set; }

        [SettingsProperty("a71fb65f318cab58b627be8b952dffd0")]
        public string FacebookSecretKey { get; set; }
    }
}