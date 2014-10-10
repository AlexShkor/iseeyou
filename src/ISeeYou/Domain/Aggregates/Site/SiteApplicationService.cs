using ISeeYou.Domain.Aggregates.Site.Commands;
using ISeeYou.Platform.Dispatching.Interfaces;
using ISeeYou.Platform.Domain.Interfaces;

namespace ISeeYou.Domain.Aggregates.Site
{
    public class SiteApplicationService : IMessageHandler
    {
        private readonly IRepository<SiteAggregate> _repository;

        public SiteApplicationService(IRepository<SiteAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateSite c)
        {
            _repository.Perform(c.Id, a => a.Create(c));
        }

        public void Handle(SetSiteSettings c)
        {
            _repository.Perform(c.Id, a => a.UpdateSettings(c));
        }

        public void Handle(StartScheduler c)
        {
            _repository.Perform(c.Id, a => a.StartScheduler());
        }

        public void Handle(StopScheduler c)
        {
            _repository.Perform(c.Id, a => a.StopScheduler(c.Restart));
        }
    }
}