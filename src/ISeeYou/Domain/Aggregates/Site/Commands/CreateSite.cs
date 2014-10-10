using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Site.Commands
{
    public class CreateSite : Command
    {
        public string Id { get; set; }
    }
}