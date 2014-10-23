using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ISeeYou.Documents;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.ViewServices;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Schedulers
{
    public class SourcesScheduler
    {
        private readonly PhotoDocumentsService _photosService;
        private readonly SourceStatsViewService _sourceStatsService;
        private readonly RabbitMqPublisher _publisher;
        private TimeSpan _delay;
        const int AvarageNewPhotos = 10;

        public SourcesScheduler(PhotoDocumentsService photosService, SourceStatsViewService sourceStatsService, SiteSettings settings)
        {
            _photosService = photosService;
            _sourceStatsService = sourceStatsService;
            _publisher = new RabbitMqPublisher(settings.RabbitHost, settings.RabbitUser, settings.RabbitPwd, settings.SourcesQueue);
            _delay = TimeSpan.FromMinutes(20);
        }

        private void Start()
        {
            while (true)
            {
                var items = _sourceStatsService.Items.Find(Query<SourceStats>.LT(x => x.NextFetching, DateTime.UtcNow)).OrderBy(x => x.NextFetching);
                var counter = 0;
                foreach (var source in items)
                {
                    if (source.FetchingStarted > DateTime.UtcNow)
                    {
                        continue;
                    }
                    counter++;
                    _sourceStatsService.Set(source.SourceId, x => x.FetchingStarted, DateTime.UtcNow);
                    _publisher.Publish(new SourceFetchEvent
                    {
                        Payload = new SourceFetchPayload()
                        {
                            UserId = source.SourceId,
                            New = source.FetchedFirstTime.HasValue,
                            Published = DateTime.UtcNow
                        }
                    });
                    long newPhotosCount = AvarageNewPhotos - 1;
                    var photosAddedStartDate = DateTime.UtcNow.AddHours(-48);
                    if (!source.FetchedFirstTime.HasValue)
                    {
                        _sourceStatsService.Set(source.SourceId, x => x.FetchedFirstTime, DateTime.UtcNow);
                    }
                    else
                    {
                        newPhotosCount = _photosService.GetPhotosCount(source.SourceId, photosAddedStartDate);
                        if (newPhotosCount < AvarageNewPhotos && source.FetchedFirstTime < photosAddedStartDate)
                        {
                            newPhotosCount = AvarageNewPhotos;
                        }
                    }
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(_delay.TotalSeconds * ((double)AvarageNewPhotos / (newPhotosCount + 1)));
                    _sourceStatsService.Set(source.SourceId, view => view.NextFetching, nextFetchingDate);
                }
                Console.WriteLine("{0} sources analyzed", counter);
                if (counter == 0)
                {
                    Thread.Sleep(2000);
                }
            }
        }

        public static void StartNew(IContainer container)
        {
            var sourcesScheduler = container.GetInstance<SourcesScheduler>();
            sourcesScheduler.Start();
        }
    }
}