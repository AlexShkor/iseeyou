using ISeeYou.Platform.Settings;

namespace ISeeYou
{
    public class SiteSettings
    {
        public const string SiteId = "ISeeYou";

        //[SettingsProperty("mongo.events_connection_string")]
        //public string MongoEventsConnectionString { get; set; }

        //[SettingsProperty("mongo.views_connection_string")]
        public string MongoViewConnectionString
        {
            get { return "mongodb://super_admin:pSS5HXsjt231izA@178.62.181.53:27017/spypie_prod"; }
        }

        [SettingsProperty("fetcher.token")]
        public string FetcherToken { get; set; }

        [SettingsProperty("threads")]
        public string Threads { get; set; }
            
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

        public string RabbitHost
        {
            get { return "dubina.by"; }
        }

        public string RabbitUser
        {
            get { return "spypie"; }
        }

        public string RabbitPwd
        {
            get { return "GM9SGQoLngSaJYZ"; }
        }

        public string PhotosQueue
        {
            get { return "spypie_photos"; }
        }

        public string SourcesQueue
        {
            get { return "spypie_sources"; }
        }

        public string BraintreeMerchantId
        {
            get { return "tp9ryz3qsdpww86c"; }
        }

        public string BraintreePublicKey
        {
            get { return "k98xvzds49wbv2z9"; }
        }

        public string BraintreePrivateKey
        {
            get { return "e7b73893964c0e1ba464946f22c7646a"; }
        }

        public decimal BraintreeTransactionAmount
        {
            get { return 40m; }
        }

        public string BraintreeSubscribtionPlanId
        {
            get { return @"dsbm"; }
        }

        public Braintree.Environment BraintreeEnvironment
        {
            get { return Braintree.Environment.SANDBOX; }
        }
    }
}