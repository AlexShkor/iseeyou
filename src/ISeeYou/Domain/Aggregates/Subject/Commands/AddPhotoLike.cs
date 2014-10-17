using System;
using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Domain.Aggregates.Subject.Commands
{
    public class AddPhotoLike: Command
    {
        public int SubjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PhotoId { get; set; }
        public int SourceId { get; set; }
        public string Image { get; set; }
        public string ImageBig { get; set; }
    }
}