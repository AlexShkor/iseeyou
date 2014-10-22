using System;
using System.Linq;
using System.Linq.Expressions;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.ViewServices;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Schedulers
{
    public class PhotoScheduler
    {
        private readonly PhotoDocumentsService _photosService;
        private readonly RabbitMqPublisher _publisher;
        private TimeSpan _delay = TimeSpan.FromMinutes(10);
        const int AvarageLikesForSource = 20;

        public PhotoScheduler(PhotoDocumentsService photosService)
        {
            this._photosService = photosService;
            const string host = "dubina.by";
            const string user = "spypie";
            const string pwd = "GM9SGQoLngSaJYZ";
            const string exchangeName = "spypie_photos";
            _publisher = new RabbitMqPublisher(host, user, pwd, exchangeName);
        }

        private void Start()
        {
            while (true)
            {
                var items = _photosService.Items.Find(Query<PhotoDocument>.LT(x => x.NextFetching, DateTime.UtcNow)).OrderBy(x => x.NextFetching);
                var counter = 0;
                foreach (var photo in items)
                {
                    if (photo.FetchingStarted > DateTime.UtcNow)
                    {
                        continue;
                    }
                    counter++;
                    _photosService.Set(photo.Id, x => x.FetchingStarted, DateTime.UtcNow);
                    _publisher.Publish(new PhotoFetchEvent
                    {
                        Payload = new PhotoFetchPayload
                        {
                            UserId = photo.SourceId,
                            PhotoId = photo.PhotoId,
                            DocId = photo.Id,
                            New = photo.FetchedFirstTime,
                            Published = DateTime.UtcNow
                        }
                    });
                    var likesForSource = 20;
                    if (!photo.FetchedFirstTime.HasValue)
                    {
                        _photosService.Set(photo.Id, x => x.FetchedFirstTime, DateTime.UtcNow);
                    }
                    //use also multiplier from source likes found data
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(_delay.TotalSeconds * GetFetchingMultiplyer(photo.Created));
                    _photosService.Set(photo.Id, x => x.NextFetching, nextFetchingDate);
                }
                Console.WriteLine("{0} photos analyzed", counter);
            }
        }

        public static void StartNew(IContainer container)
        {
            var obj = container.GetInstance<PhotoScheduler>();
            obj.Start();
        }

        private static int GetFetchingMultiplyer(DateTime created)
        {
            var now = DateTime.UtcNow;
            if (created > now.AddDays(-2))
            {
                return 1;
            }
            if (created > now.AddDays(-4))
            {
                return 2;
            }
            if (created > now.AddDays(-10))
            {
                return 5;
            }
            if (created > now.AddDays(-30))
            {
                return 7;
            }
            return 10;
        }
    }
}