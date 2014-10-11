using System;
using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Subject
{
    public class PhotoLikeAdded : Event
    {
        public int SubjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long PhotoId { get; set; }
        public int SourceId { get; set; }
        public string Image { get; set; }
    }
}