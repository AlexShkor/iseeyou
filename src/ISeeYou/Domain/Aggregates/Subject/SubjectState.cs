using System.Collections.Generic;
using ISeeYou.Platform.Domain;

namespace ISeeYou.Domain.Aggregates.Subject
{
    public class SubjectState: AggregateState
    {
        public string Id { get; set; }

        public SortedSet<long> LikedPhotos { get; set; }  

        public SubjectState()
        {
            LikedPhotos = new SortedSet<long>();
            On((SubjectCreated e) => Id = e.Id);
            On((PhotoLikeAdded e) => LikedPhotos.Add(e.PhotoId));
        }
    }
}