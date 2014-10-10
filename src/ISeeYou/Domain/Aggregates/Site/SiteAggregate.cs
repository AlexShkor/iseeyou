using System;
using ISeeYou.Domain.Aggregates.Site.Commands;
using ISeeYou.Domain.Aggregates.Site.Events;
using ISeeYou.Platform.Domain;
using ISeeYou.Platform.Extensions;

namespace ISeeYou.Domain.Aggregates.Site
{
    public class SiteAggregate : Aggregate<SiteState>
    {
        public void Create(CreateSite c)
        {
            if (State.Id.HasValue())
            {
                throw new InvalidOperationException("Site object is already created");
            }
            Apply(new SiteCreated
            {
                Id = c.Id,
            });
        }

        public void UpdateSettings(SetSiteSettings c)
        {
            Apply(new SiteSettingsUpdated
            {
                Id = State.Id,
                SmtpSettings = c.SmtpSettings,
            });
        }

        public void StartScheduler()
        {
            Apply(new SchedulerStarted()
            {
                Id = State.Id,
            });
        }

        public void StopScheduler(bool restart)
        {
            Apply(new SchedulerStopped()
            {
                Id = State.Id,
                Restart = restart
            });
        }

    }
}