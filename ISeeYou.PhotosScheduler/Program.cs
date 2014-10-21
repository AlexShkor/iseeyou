using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.ViewServices;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.PhotosScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "dubina.by";
            var user = "spypie";
            var pwd = "GM9SGQoLngSaJYZ";
            var exchangeName = "spypie_photos";

            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var photosService = container.GetInstance<PhotoDocumentsService>();
            Action<string, Expression<Func<PhotoDocument, DateTime?>>, DateTime> setSource = (id, expr, date) =>
                photosService.Items.Update(Query<PhotoDocument>.EQ(x => x.Id, id),
                        Update<PhotoDocument>.Set(expr, date));
            Action<string, Expression<Func<PhotoDocument, DateTime?>>> setDateTime =
                (id, expr) => setSource(id, expr, DateTime.UtcNow);
            var publisher = new RabbitMqPublisher(host, user, pwd, exchangeName);
            var delay = TimeSpan.FromMinutes(10);
            const int avarageLikesForSource = 20;
            while (true)
            {
                var items = photosService.Items.Find(Query<PhotoDocument>.LT(x => x.NextFetching, DateTime.UtcNow)).OrderBy(x=> x.NextFetching);
                var counter = 0;
                foreach (var photo in items)
                {
                    if (photo.FetchingStarted > DateTime.UtcNow)
                    {
                        continue;
                    }
                    counter++;
                    setDateTime(photo.Id, x => x.FetchingStarted);
                    publisher.Publish(new PhotoFetchEvent
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
                        setDateTime(photo.Id, x => x.FetchedFirstTime);
                    }
                    //use also multiplier from source likes found data
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(delay.TotalSeconds * GetFetchingMultiplyer(photo.Created));
                    setSource(photo.Id, view => view.NextFetching, nextFetchingDate);
                }
                Console.WriteLine("{0} photos analyzed", counter);
            }
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
