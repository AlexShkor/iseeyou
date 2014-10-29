using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ISeeYou.ViewServices;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Schedulers
{
    public class PhotoScheduler
    {
        private readonly PhotoDocumentsService _photosService;
        private readonly PhotoPublisher _publisher;
        private readonly FetchSettings _fetchSettings;
        const int AvarageLikesForSource = 20;


        public PhotoScheduler(PhotoDocumentsService photosService, PhotoPublisher publisher, FetchSettings fetchSettings)
        {
            _publisher = publisher;
            _fetchSettings = fetchSettings;
            _photosService = photosService;
        }

        private void Start()
        {
            while (true)
            {
                if (_fetchSettings.IsAppDisabled())
                {
                    continue;
                }
                var chunkSize = 500;
                var items =
                    _photosService.Items.Find(Query<PhotoDocument>.LT(x => x.NextFetching, DateTime.UtcNow))
                        .SetSortOrder(SortBy<PhotoDocument>.Ascending(x => x.NextFetching));
                var counter = 0;
                var delaySum = TimeSpan.Zero;
                var stopwatch = new Stopwatch();
                var photosPerSecond = 0;
                stopwatch.Start();
                foreach (var photo in items)
                {
                    if (_fetchSettings.IsAppDisabled())
                    {
                        break;
                    }
                    if (photo.NextFetching > DateTime.UtcNow)
                    {
                        continue;
                    }
                    //if (photo.FetchingStarted > DateTime.UtcNow)
                    //{
                    //    continue;
                    //}
                    //_photosService.Set(photo.Id, x => x.FetchingStarted, DateTime.UtcNow);
                 
                    counter++;
                    _publisher.Publish(photo.PhotoId, photo.SourceId, photo.Id);
                    var likesForSource = 20;
                    var additionalDellay = TimeSpan.Zero;
                    if (!photo.FetchedFirstTime.HasValue)
                    {
                        _photosService.Set(photo.Id, x => x.FetchedFirstTime, DateTime.UtcNow);
                        additionalDellay = TimeSpan.FromMinutes(20);
                    }
                    //use also multiplier from source likes found data
                    var delay = GetFetchInterval(photo);
                    delaySum += delay;
                    var nextFetchingDate = DateTime.UtcNow + delay + additionalDellay;
                    _photosService.Items.Update(Query.EQ("_id", BsonValue.Create(photo.Id)),
                        Update<PhotoDocument>.Set(x => x.NextFetching, nextFetchingDate)
                            .Set(x => x.FetchingEnd, DateTime.UtcNow));

                    photosPerSecond++;
                    if (stopwatch.ElapsedMilliseconds >= 1000)
                    {
                        Console.WriteLine("Photos: {0}; Elapsed time: {1} mls", photosPerSecond, stopwatch.ElapsedMilliseconds);
                        if (photosPerSecond > 0)
                        {
                            Console.WriteLine("Avarage Delay: {0} seconds", delaySum.TotalSeconds / photosPerSecond);
                        }
                        stopwatch.Reset();
                        stopwatch.Start();
                        photosPerSecond = 0;
                        delaySum = TimeSpan.Zero;
                    }
                }
                Console.WriteLine("{0} photos analyzed", counter);
                if (counter < chunkSize)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public static void StartNew(IContainer container)
        {
            var obj = container.GetInstance<PhotoScheduler>();
            obj.Start();
        }



        private TimeSpan GetFetchInterval(PhotoDocument photo)
        {
            var settings = _fetchSettings.GetFetchSettings();
            var photoAge = (DateTime.UtcNow - photo.Created).TotalDays;
            var category = settings.Categories.OrderBy(x => x.Age).Where(x => x.Age > photoAge).FirstOrDefault();
            var ratio = category == null ? 1.0 : category.Ratio;
            var seconds = settings.DelayBase * ratio;
            return TimeSpan.FromSeconds(seconds);
        }
    }
}