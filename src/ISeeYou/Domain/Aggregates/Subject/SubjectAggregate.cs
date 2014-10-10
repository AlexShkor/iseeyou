using System;
using ISeeYou.Domain.Aggregates.Site;
using ISeeYou.Platform.Domain;
using ISeeYou.Platform.Domain.Interfaces;
using ISeeYou.Platform.Extensions;

namespace ISeeYou.Domain.Aggregates.Subject
{
    public class SubjectAggregate : Aggregate<SubjectState>
    {
        public void Create(string id)
        {
            if (State.Id.HasValue())
            {
                throw new InvalidOperationException("Already created;");
            }
            Apply(new SubjectCreated
            {
                Id = id
            });
        }

        public void AddLike(string sourceId, DateTime dateTime)
        {
            
        }
    }
}