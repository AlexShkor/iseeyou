using ISeeYou.Domain.Aggregates.Site.Events;
using ISeeYou.Platform.Domain;

namespace ISeeYou.Domain.Aggregates.Site
{
    public sealed class SiteState : AggregateState
    {
        public string Id { get; set; }

        public SiteState()
        {
            On((SiteCreated e) => Id = e.Id);
        }
    }
}