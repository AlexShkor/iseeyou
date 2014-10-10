using System.Globalization;
using ISeeYou.Domain.Aggregates.Site;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.Platform.Dispatching.Interfaces;
using ISeeYou.Platform.Domain.Interfaces;

namespace ISeeYou.Domain.Aggregates.Subject
{
    public class SiteApplicationService : IMessageHandler
    {
        private readonly IRepository<SubjectAggregate> _repository;

        public SiteApplicationService(IRepository<SubjectAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateSubject c)
        {
            _repository.Perform(c.Id, aggregate => aggregate.Create(c.Id));
        }

        public void Handle(AddPhotoLike c)
        {
            _repository.Perform(c.Id, aggregate => aggregate.AddPhotoLike(c));
        }
    }
}