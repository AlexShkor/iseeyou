using System;
using ISeeYou.Domain.Aggregates.Site;
using ISeeYou.Domain.Aggregates.Subject.Commands;
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

        public void AddPhotoLike(AddPhotoLike c)
        {
            if (State.LikedPhotos.Contains(c.PhotoId))
            {
                return;
            }
            Apply(new PhotoLikeAdded
            {
                Id = State.Id,
                Image = c.Image,
                EndDate = c.EndDate,
                PhotoId = c.PhotoId,
                SourceId = c.SourceId,
                StartDate = c.StartDate,
                SubjectId = c.SubjectId
            });
        }
    }
}