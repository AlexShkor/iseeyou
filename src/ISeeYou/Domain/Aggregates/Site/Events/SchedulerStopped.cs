using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Site.Events
{
    public class SchedulerStopped: Event
    {
        public bool Restart { get; set; }
    }
}