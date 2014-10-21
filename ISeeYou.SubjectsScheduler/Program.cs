using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.SubjectsScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var subjectsService = container.GetInstance<SubjectViewService>();
            var sources = container.GetInstance<SourcesViewService>();
            var sourceStats = container.GetInstance<SourceStatsViewService>();
            var api = new VkApi();
            Action<int, Expression<Func<SubjectView, DateTime?>>, DateTime> setSubject = (id, expr, date) =>
                subjectsService.Items.Update(Query<SubjectView>.EQ(x => x.Id, id),
                        Update<SubjectView>.Set(expr, date));
            Action<int, Expression<Func<SubjectView, DateTime?>>> setDateTime =
                (id, expr) => setSubject(id, expr, DateTime.UtcNow);
            var fields = new[] { "sex" };
            var delay = TimeSpan.FromMinutes(20);
            const int avarageNewSources = 3;
            while (true)
            {
                var items = subjectsService.Items.Find(Query<SubjectView>.LT(x => x.NextFetching, DateTime.UtcNow));
                var counter = 0;
                foreach (var subjectView in items)
                {
                    var id = subjectView.Id.ToString(CultureInfo.InvariantCulture);
                    if (subjectView.FetchingStarted > DateTime.UtcNow)
                    {
                        continue;
                    }
                    counter++;
                    setDateTime(subjectView.Id, x => x.FetchingStarted);
                    var friends = api.GetUserFriends(id, fields);
                    var sourceIds = sources.GetSourceIdsFor(subjectView.Id).ToList();
                    var newSources = friends.Where(x => !sourceIds.Contains(x.UserId));
                    foreach (var newSource in newSources)
                    {
                        sourceStats.InsertAsync(new SourceStats
                        {
                            SourceId = newSource.UserId,
                            NextFetching = DateTime.UtcNow
                        });
                        sources.InsertAsync(new SourceDocument()
                        {
                            SourceId = newSource.UserId,
                            SubjectId = subjectView.Id,
                            Added = DateTime.UtcNow,
                            New = subjectView.FetchedFirstTime.HasValue
                        });
                    }
                    long newSourcesCount = avarageNewSources - 1;
                    if (!subjectView.FetchedFirstTime.HasValue)
                    {
                        setDateTime(subjectView.Id, x => x.FetchedFirstTime);
                    }
                    else
                    {
                        newSourcesCount = sources.GetSourcesCount(subjectView.Id, DateTime.UtcNow.AddHours(-48));
                    }
                    var nextFetchingDate = DateTime.UtcNow + TimeSpan.FromSeconds(delay.TotalSeconds * ((double)avarageNewSources/(newSourcesCount + 1) ));
                    setSubject(subjectView.Id, view => view.NextFetching, nextFetchingDate);
                    setDateTime(subjectView.Id, x => x.FetchingEnded);
                }
                Console.WriteLine("{0} subjects analyzed", counter);
            }
        }
    }
}
