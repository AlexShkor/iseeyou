using System.Collections.Generic;
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

        public PhotoFetchSettings PhotoFetchSettings { get; set; }

        public SiteView()
        {
            SmtpSettings = new SmtpSettingsData();

        }



    }

    public class PhotoFetchSettings
    {
        public bool Disabled { get; set; }
        public int DelayBase { get; set; }
        public List<PhotoCategory> Categories { get; set; }
    }

    public class PhotoCategory
    {
        public int Age { get; set; }
        public double Ratio { get; set; }
    }

    public class ChainItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}