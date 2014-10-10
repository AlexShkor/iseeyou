using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Site.Commands
{
    public class StopScheduler:Command
    {
        public bool Restart { get; set; }
    }
}