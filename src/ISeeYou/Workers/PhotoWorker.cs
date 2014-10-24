using System;
using System.Collections.Generic;
using System.Linq;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using StructureMap;

namespace ISeeYou.Workers
{
    public class PhotoWorker
    {
        public static void Start()
        {
            var updateSubjectsInterval = TimeSpan.FromMinutes(1);

            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var api = new VkApi();


            const string type = "photo";
            var settings = container.GetInstance<SiteSettings>();
            var consumer = new RabbitMqConsumer<PhotoFetchEvent>(settings.RabbitHost, settings.RabbitUser, settings.RabbitPwd, settings.PhotosQueue);
            var subjectsService = container.GetInstance<SubjectViewService>();
            var photosService = container.GetInstance<PhotoDocumentsService>();
            var sources = container.GetInstance<SourceStatsViewService>();
            var events = ObjectFactory.Container.GetInstance<EventsViewService>();
            var trakingMarks = ObjectFactory.Container.GetInstance<TrackingMarksViewService>();
            List<int> subjectIds = null;
            ILookup<int, DateTime> subjectTrackingDateLookup = null;
            var lastSubjectsUpdate = DateTime.MinValue;

            Action updateSubjects = () =>
            {
                if (DateTime.UtcNow > lastSubjectsUpdate + updateSubjectsInterval)
                {
                    var subjects = subjectsService.GetAll().Select(x => new { x.Id, x.TrackingStarted }).ToList();
                    subjectIds = subjects.Select(x => x.Id).ToList();
                    subjectTrackingDateLookup = subjects.ToLookup(x => x.Id, x => x.TrackingStarted);
                    lastSubjectsUpdate = DateTime.UtcNow;
                    Console.WriteLine("Subjects list updated");
                }
            };

            var subjectsLoaded = DateTime.UtcNow;
            var counter = 0;
            consumer.On += photoEvent =>
            {
                updateSubjects();
                var photo = photoEvent.Payload;
                counter ++;
                Console.WriteLine("Photo processing started: {0}. id: {1}", counter, photo.DocId);
                var photoId = photo.DocId;
                var result = api.Likes(photo.PhotoId, photo.UserId);
                if (result != null && result.Any())
                {
                    var intersect = result.Intersect(subjectIds);
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
                            var likedDate = doc.Created;
                            //var likedDate = doc.FetchingEnd;
                            ////new added photo or previously was not liked
                            //if (likedDate == null)
                            //{
                            //    //was not liked
                            //    likedDate = trakingMarks.GetSourceMark(photo.UserId);
                            //    //newly created
                            //    if (likedDate == null)
                            //    {
                            //        likedDate = doc.Created;
                            //    }
                            //}
                            //var trackingDate = subjectTrackingDateLookup[subjectId].FirstOrDefault();
                            ////subject was not traked
                            //if (trackingDate > doc.FetchingEnd)
                            //{
                            //    likedDate = doc.Created;
                            //}


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
                        subjectsService.Inc(subjectId, x => x.Likes, 1);
                        sources.Inc(photo.UserId, x => x.Likes, 1);
                        photosService.Inc(photo.DocId, x => x.Likes, 1);
                    }
                }
                //photosService.Items.Update(Query<PhotoDocument>.EQ(x => x.Id, photoId),
                //    Update<PhotoDocument>.Set(x => x.FetchingEnd, DateTime.UtcNow));
                //TODO: log pgoto fetching stats
            };
            consumer.Start();
        }
    }
}