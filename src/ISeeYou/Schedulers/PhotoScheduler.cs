using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Schedulers
{
    public class PhotoScheduler
    {
        private readonly PhotoDocumentsService _photosService;
        private readonly PhotoPublisher _publisher;
        private readonly SitesViewService _siteService;
        const int AvarageLikesForSource = 20;

        private DateTime _lastFetchSettingsUpdate;
        private TimeSpan _fetchSettingsUpdateInterval = TimeSpan.FromMinutes(1);
        private PhotoFetchSettings _fetchSettings;

        public PhotoScheduler(PhotoDocumentsService photosService, SitesViewService siteService, PhotoPublisher publisher)
        {
            _siteService = siteService;
            _publisher = publisher;
            _photosService = photosService;
        }

        private void Start()
        {
            while (true)
            {
                var chunkSize = 500;
                var items = _photosService.Items.Find(Query<PhotoDocument>.LT(x => x.NextFetching, DateTime.UtcNow)).SetSortOrder(SortBy<PhotoDocument>.Ascending(x => x.NextFetching)).SetLimit(chunkSize).ToList();
                var counter = 0;
                var delaySum = TimeSpan.Zero;
                foreach (var photo in items)
                {
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
                }
                Console.WriteLine("{0} photos analyzed", counter);
                if (counter > 0)
                {
                    Console.WriteLine("Avarage Delay: {0} seconds", delaySum.TotalSeconds / counter);
                }
                if (items.Count() < chunkSize)
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

        private PhotoFetchSettings GetFetchSettings()
        {
            if (DateTime.UtcNow < _lastFetchSettingsUpdate + _fetchSettingsUpdateInterval)
                return _fetchSettings;

            _lastFetchSettingsUpdate = DateTime.UtcNow;
            
            var siteSettings = _siteService.GetSite();
            if (siteSettings == null)
            {
                siteSettings = new SiteView
                {
                    Id = SiteSettings.SiteId,
                    PhotoFetchSettings = DefaultFetchSettings()
                };
                _siteService.InsertAsync(siteSettings);
            }

            if (siteSettings.PhotoFetchSettings == null)
            {
                siteSettings.PhotoFetchSettings = DefaultFetchSettings();
                _siteService.Save(siteSettings);
            }

            return _fetchSettings = siteSettings.PhotoFetchSettings;
        }

        private TimeSpan GetFetchInterval(PhotoDocument photo)
        {
            var settings = GetFetchSettings();

            var photoAge = (DateTime.UtcNow - photo.Created).TotalDays;

            var category = settings.Categories.OrderBy(x => x.Age).Where(x => x.Age > photoAge).FirstOrDefault();

            var ratio = category == null ? 1.0 : category.Ratio;
            var seconds = settings.DelayBase*ratio;
            return TimeSpan.FromSeconds(seconds);
        }


        private static PhotoFetchSettings DefaultFetchSettings()
        {
            return new PhotoFetchSettings
            {
                DelayBase = 50,
                Disabled = false,
                Categories = new List<PhotoCategory>
                {
                    new PhotoCategory {Age = 2, Ratio = 5},
                    new PhotoCategory {Age = 4, Ratio = 6},
                    new PhotoCategory {Age = 10, Ratio = 7},
                    new PhotoCategory {Age = 30, Ratio = 8},
                    new PhotoCategory {Age = int.MaxValue, Ratio = 10},
                }
            };
        }
    }
}