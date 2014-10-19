using System;
using System.Linq;
using ISeeYou.MQ;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using StructureMap;

namespace ISeeYou.Worker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var api = new VkApi();
            const string host = "dubina.by";
            const string user = "spypie";
            const string pwd = "GM9SGQoLngSaJYZ";
            const string fetchingExchange = "spypie_photos";

            const string type = "photo";
            var subjectAddedConsumer = new RabbitMqConsumer<PhotoFetchEvent>(host, user, pwd, fetchingExchange);
            var subjectsService = container.GetInstance<SubjectViewService>();
            var photosService = container.GetInstance<PhotoDocumentsService>();
            var events = ObjectFactory.Container.GetInstance<EventsViewService>();
            var trakingMarks = ObjectFactory.Container.GetInstance<TrackingMarksViewService>();
            var subjects = subjectsService.GetAllIds();
            var subjectsLoaded = DateTime.UtcNow;
            var counter = 0;
            subjectAddedConsumer.On += photoEvent =>
            {
                counter ++;
                if (counter > 100)
                {
                    subjects = subjectsService.GetAllIds();
                    subjectsLoaded = DateTime.UtcNow;
                    counter = 0;
                }
                var photo = photoEvent.Payload;
                var photoId = type + photo.UserId + "_" + photo.Id;
                var result = api.Likes(photo.Id, photo.UserId);
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
                        if (events.Items.FindOneById(photoId) == null)
                        {
                            var id = subjectId + "_" + photo.UserId + "_" + type + photo.Id;
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
                            events.Insert(new EventView
                            {
                                DocId = id,
                                PhotoId = photo.Id,
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
                    photosService.Items.Update(Query<PhotoDocument>.EQ(x => x.Id, photoId),
                        Update<PhotoDocument>.Set(x => x.FetchingEnd, DateTime.UtcNow));
                    //TODO: set next fetching date
                }
                //TODO: log pgoto fetching stats
            };
            subjectAddedConsumer.Start();
        }
    }

    public class PhotoFetchEvent : RabbitEventBase
    {
        public PhotoFetchPayload Payload { get; set; }

        public override string RoutingKey
        {
            get { return "photo_fetch"; }
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(Payload);
        }

        public override void Deserialize(string message)
        {
            Payload = JsonConvert.DeserializeObject<PhotoFetchPayload>(message);
        }
    }

    public class PhotoFetchPayload
    {
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
