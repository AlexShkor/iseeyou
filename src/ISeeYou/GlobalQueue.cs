using System;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using StructureMap;

namespace ISeeYou
{
    public class GlobalQueue
    {
        public static void Send(AddPhotoLike c)
        {
            var events = ObjectFactory.Container.GetInstance<EventsViewService>();
            var trakingMarks = ObjectFactory.Container.GetInstance<TrackingMarksViewService>();
            if (events.Items.FindOneById(c.PhotoId) == null)
            {
                const string type = "photo";
                var id = c.SubjectId + "_" + c.SourceId + "_" + type + c.PhotoId;
                var tracked = trakingMarks.GetById(id);
                events.Items.Save(new EventView
                {
                    DocId = id,
                    PhotoId = c.PhotoId,
                    Image = c.Image,
                    ImageBig = c.ImageBig,
                    SubjectId = c.SubjectId,
                    EndDate = c.EndDate,
                    SourceId = c.SourceId,
                    StartDate = tracked != null ? tracked.Tracked : c.StartDate,
                    Type = type
                });
                trakingMarks.Items.Insert(new TrackingMark {Id = id, Tracked = DateTime.UtcNow});
            }
        }
    }
}