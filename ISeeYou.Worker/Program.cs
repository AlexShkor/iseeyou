using System;
using System.Globalization;
using System.Linq;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.MQ;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
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
            var subjectAddedConsumer = new RabbitMqConsumer<PhotoFetchEvent>(host, user, pwd, fetchingExchange);
            var subjectsService = container.GetInstance<SubjectViewService>();
            var photosService = container.GetInstance<PhotoDocumentsService>();
            var subjects = subjectsService.GetAll().Select(x => x.Id).ToList();
            subjectAddedConsumer.SetPeriodicalAction(100,
                () => subjects = subjectsService.GetAll().Select(x => x.Id).ToList());
            subjectAddedConsumer.On += photoEvent =>
            {
                var photo = photoEvent.Payload;
                var photoId = photo.UserId + "_" + photo.Id;
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
                        GlobalQueue.Send(new AddPhotoLike
                        {
                            Id = subjectId.ToString(CultureInfo.InvariantCulture),
                            SubjectId = subjectId,
                            StartDate = doc.Created,
                            EndDate = DateTime.UtcNow,
                            PhotoId = photo.Id,
                            Image = doc.Image,
                            ImageBig = doc.ImageBig
                        });
                    }
                }

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
