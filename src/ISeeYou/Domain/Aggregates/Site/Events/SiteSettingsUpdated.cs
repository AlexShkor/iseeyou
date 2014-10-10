using ISeeYou.Domain.Aggregates.Site.Data;
using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Site.Events
{
    public class SiteSettingsUpdated : Event
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}