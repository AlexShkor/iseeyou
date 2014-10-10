using ISeeYou.Domain.Aggregates.Site.Data;
using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Site.Commands
{
    public class SetSiteSettings : Command
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}