﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ISeeYou.Documents;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Schedulers
{
    public class SourcesScheduler
    {
        private readonly PhotoDocumentsService _photosService;
        private readonly SourceStatsViewService _sourceStatsService;
        private readonly SourcePublisher _publisher;
        private readonly FetchSettings _fetchSettings;
        private TimeSpan _delay;
        const int AverageNewPhotos = 10;

        public SourcesScheduler(PhotoDocumentsService photosService, SourceStatsViewService sourceStatsService, SourcePublisher publisher, FetchSettings fetchSettings)
        {
            _photosService = photosService;
            _sourceStatsService = sourceStatsService;
            _publisher = publisher;
            _fetchSettings = fetchSettings;

            _delay = TimeSpan.FromMinutes(10);
        }

        private void Start()
        {
            Console.WriteLine("Started");
            while (true)
            {
                if (_fetchSettings.IsAppDisabled())
                {
                    continue;
                }
                var chunkSize = 500;
                var items = _sourceStatsService.Items.Find(Query<SourceStats>.LT(x => x.NextFetching, DateTime.UtcNow)).SetSortOrder(SortBy<SourceStats>.Ascending(x => x.NextFetching)).SetLimit(chunkSize).ToList();
                var counter = 0;
                foreach (var source in items)
                {
                    counter++;
                    _publisher.Publish(source.SourceId);
                    long newPhotosCount = AverageNewPhotos - 1;
                    var photosAddedStartDate = DateTime.UtcNow.AddHours(-48);
                    if (!source.FetchedFirstTime.HasValue)
                    {
                        _sourceStatsService.Set(source.SourceId, x => x.FetchedFirstTime, DateTime.UtcNow);
                    }
                    else
                    {
                        //newPhotosCount = _photosService.GetPhotosCount(source.SourceId, photosAddedStartDate);
                        //if (newPhotosCount < AverageNewPhotos && source.FetchedFirstTime < photosAddedStartDate)
                        //{
                        //    newPhotosCount = AverageNewPhotos;
                        //}
                    }
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(_delay.TotalSeconds * ((double)AverageNewPhotos / (newPhotosCount + 1)));
                    _sourceStatsService.Set(source.SourceId, view => view.NextFetching, nextFetchingDate);
                }
                Console.WriteLine("{0} sources analyzed", counter);

                if (items.Count() < chunkSize)
                {
                    Thread.Sleep(1000);
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