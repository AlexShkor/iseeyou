using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using StructureMap;

namespace ISeeYou.SourceScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "dubina.by";
            var user = "spypie";
            var pwd = "GM9SGQoLngSaJYZ";
            var exchangeName = "spypie_sources";

            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var photosService = container.GetInstance<PhotoDocumentsService>();
            var sourceStatsService = container.GetInstance<SourceStatsViewService>();
            Action<int, Expression<Func<SourceStats, DateTime?>>, DateTime> setSource = (id, expr, date) =>
                sourceStatsService.Items.Update(Query<SourceStats>.EQ(x => x.SourceId, id),
                        Update<SourceStats>.Set(expr, date));
            Action<int, Expression<Func<SourceStats, DateTime?>>> setDateTime =
                (id, expr) => setSource(id, expr, DateTime.UtcNow);
            var publisher = new RabbitMqPublisher(host, user, pwd, exchangeName);
            var delay = TimeSpan.FromMinutes(20);
            const int avarageNewPhotos = 10;
            while (true)
            {
                var items = sourceStatsService.Items.Find(Query<SourceStats>.LT(x => x.NextFetching, DateTime.UtcNow)).OrderBy(x=> x.NextFetching);
                var counter = 0;
                foreach (var source in items)
                {
                    if (source.FetchingStarted > DateTime.UtcNow)
                    {
                        continue;
                    }
                    counter++;
                    setDateTime(source.SourceId, x => x.FetchingStarted);
                    publisher.Publish(new SourceFetchEvent
                    {
                        Payload = new SourceFetchPayload()
                        {
                            UserId = source.SourceId,
                            New = source.FetchedFirstTime.HasValue,
                            Published = DateTime.UtcNow
                        }
                    });
                    long newPhotosCount = avarageNewPhotos - 1;
                    var photosAddedStartDate = DateTime.UtcNow.AddHours(-48);
                    if (!source.FetchedFirstTime.HasValue)
                    {
                        setDateTime(source.SourceId, x => x.FetchedFirstTime);
                    }
                    else
                    {
                        newPhotosCount = photosService.GetPhotosCount(source.SourceId, photosAddedStartDate);
                        if (newPhotosCount < avarageNewPhotos && source.FetchedFirstTime < photosAddedStartDate)
                        {
                            newPhotosCount = avarageNewPhotos;
                        }
                    }
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(delay.TotalSeconds * ((double)avarageNewPhotos / (newPhotosCount + 1)));
                    setSource(source.SourceId, view => view.NextFetching, nextFetchingDate);
                }
                Console.WriteLine("{0} subjects analyzed", counter);
            }
        }
    }
}
