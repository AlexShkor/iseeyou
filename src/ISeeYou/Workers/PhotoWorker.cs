using System;
using System.Linq;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Workers
{
    public class PhotoWorker
    {
        public static void Start()
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var api = new VkApi();
     

            const string type = "photo";
            var settings = container.GetInstance<SiteSettings>();
            var consumer = new RabbitMqConsumer<PhotoFetchEvent>(settings.RabbitHost, settings.RabbitUser, settings.RabbitPwd, settings.PhotosQueue);
            var subjectsService = container.GetInstance<SubjectViewService>();
            var photosService = container.GetInstance<PhotoDocumentsService>();
            var events = ObjectFactory.Container.GetInstance<EventsViewService>();
            var trakingMarks = ObjectFactory.Container.GetInstance<TrackingMarksViewService>();
            var subjects = subjectsService.GetAllIds();
            var subjectsLoaded = DateTime.UtcNow;
            var counter = 0;
            consumer.On += photoEvent =>
            {
                counter++;
              
                if (counter > 100)
                {
                    subjects = subjectsService.GetAllIds();
                    subjectsLoaded = DateTime.UtcNow;
                    counter = 0;
                    Console.WriteLine("Subjects list updated");
                }
                var photo = photoEvent.Payload;
                Console.WriteLine("Photo processing started: {0}. id: {1}", counter, photo.DocId);
                var photoId = photo.DocId;
                var result = api.Likes(photo.PhotoId, photo.UserId);
                if (result != null && result.Any())
                {
                    var intersect = result.Intersect(subjects);
                    PhotoDocument doc = null;
                    foreach (var subjectId in intersect)
                    {
                        if (doc == null)
                        {
                            doc = photosService.GetById(photoId);
                        }
                        var eventId = subjectId + "_" + type + photo.UserId + "_" + photo.PhotoId;
                        if (events.Items.FindOneById(photoId) == null)
                        {
                            var subject = subjectsService.Items.FindOneById(subjectId);
                            var likedDate = doc.FetchingEnd;
                            //new added photo or previously was not liked
                            if (likedDate == null)
                            {
                                //was not liked
                                likedDate = trakingMarks.GetSourceMark(photo.UserId);
                                //newly created
                                if (likedDate == null)
                                {
                                    likedDate = doc.Created;
                                }
                            }
                            //subject was not traked
                            if (subject.TrackingStarted > doc.FetchingEnd)
                            {
                                likedDate = doc.Created;
                            }
                            events.InsertAsync(new EventView
                            {
                                DocId = eventId,
                                PhotoId = photo.PhotoId,
                                Image = doc.Image,
                                ImageBig = doc.ImageBig,
                                SubjectId = subjectId,
                                EndDate = DateTime.UtcNow,
                                SourceId = photo.UserId,
                                StartDate = likedDate,
                                Type = type
                            });
                        }
                    }
                }
                photosService.Items.Update(Query<PhotoDocument>.EQ(x => x.Id, photoId),
                    Update<PhotoDocument>.Set(x => x.FetchingEnd, DateTime.UtcNow));
                //TODO: log pgoto fetching stats
            };
            consumer.Start();
        }
    }
}