using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ISeeYou.Documents;
using ISeeYou.MQ;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Schedulers
{
    public class SubjectScheduler
    {

        private readonly SubjectViewService _subjectsService;
        private readonly SourcesViewService _sources;
        private readonly SourceStatsViewService _sourceStats;
        private readonly VkApi _api;

        private readonly string[] _fields = new[] { "sex" };
        private readonly TimeSpan _delay = TimeSpan.FromMinutes(10);
        private const int AverageNewSources = 3;

        public SubjectScheduler(SourceStatsViewService sourceStats, SourcesViewService sources, SubjectViewService subjectsService)
        {
            _sourceStats = sourceStats;
            _sources = sources;
            _subjectsService = subjectsService;
            _api = new VkApi();
        }

        private void Start()
        {
            while (true)
            {
                var chunkSize = 500;
                var items = _subjectsService.Items.Find(Query<SubjectView>.LT(x => x.NextFetching, DateTime.UtcNow)).SetLimit(chunkSize).ToList();
                var counter = 0;
                foreach (var subjectView in items)
                {
                    var id = subjectView.Id.ToString(CultureInfo.InvariantCulture);
                    counter++;
                    var friends = _api.GetUserFriends(id, _fields);
                    var sourceIds = _sources.GetSourceIdsFor(subjectView.Id).ToList();
                    var newSources = friends.Where(x => !sourceIds.Contains(x.UserId));
                    foreach (var newSource in newSources)
                    {
                        _sourceStats.InsertAsync(new SourceStats
                        {
                            SourceId = newSource.UserId,
                            NextFetching = DateTime.UtcNow
                        });
                        _sources.InsertAsync(new SourceDocument()
                        {
                            SourceId = newSource.UserId,
                            SubjectId = subjectView.Id,
                            Added = DateTime.UtcNow,
                            New = subjectView.FetchedFirstTime.HasValue
                        });
                    }
                    long newSourcesCount = AverageNewSources - 1;
                    if (!subjectView.FetchedFirstTime.HasValue)
                    {
                        _subjectsService.Set(subjectView.Id, x => x.FetchedFirstTime, DateTime.UtcNow);
                    }
                    else
                    {
                        newSourcesCount = _sources.GetSourcesCount(subjectView.Id, DateTime.UtcNow.AddHours(-48));
                    }
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(_delay.TotalSeconds * ((double)AverageNewSources / (newSourcesCount + 1)));
                    _subjectsService.Set(subjectView.Id, view => view.NextFetching, nextFetchingDate);
                    _subjectsService.Set(subjectView.Id, x => x.FetchingEnded, DateTime.UtcNow);
                }
                Console.WriteLine("{0} subjects analyzed", counter);

                if (items.Count() < chunkSize)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public static void StartNew(IContainer container)
        {
            var ins = container.GetInstance<SubjectScheduler>();
            ins.Start();
        }
    }
}