using System;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using MongoDB.Driver;
using StructureMap;

namespace ISeeYou
{
    public class GlobalQueue
    {
        public static void Send(AddPhotoLike c)
        {
            var events = ObjectFactory.Container.GetInstance<EventsViewService>();
            if (events.Items.FindOneById(c.PhotoId) == null)
            {
                events.Items.Save(new EventView
                {
                    Id = c.PhotoId,
                    Image = c.Image,
                    ImageBig = c.ImageBig,
                    AlbumId = c.AlbumId,
                    SubjectId = c.SubjectId,
                    EndDate = c.EndDate,
                    SourceId = c.SourceId,
                    StartDate = c.StartDate,
                    Type = "photo"
                });
            }
        }
    }
}