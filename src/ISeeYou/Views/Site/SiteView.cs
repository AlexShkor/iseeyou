using ISeeYou.Domain.Aggregates.Site.Data;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace ISeeYou.Views
{
    public class SiteView
    {
        [DocumentId, BsonId]
        public string Id { get; set; }

        public SmtpSettingsData SmtpSettings { get; set; }

        public bool SchedulerStopped { get; set; }

        public bool SchedulerRestartNeeded { get; set; }

        public SiteView()
        {
            SmtpSettings = new SmtpSettingsData();

        }



    }

    public class ChainItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}