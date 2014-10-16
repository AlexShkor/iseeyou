using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.ViewServices;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace ISeeYou.Fetching
{
    public class SourceFetcher
    {
        private readonly SourcesViewService _sources;
        private readonly FetchingStatsService _stats;
        private readonly SourceStatsViewService _sourceStats;

        public SourceFetcher(SourcesViewService sources, FetchingStatsService stats, SourceStatsViewService sourceStats)
        {
            _sources = sources;
            _stats = stats;
            _sourceStats = sourceStats;
        }

        public void Run()
        {
            var now = DateTime.UtcNow;
            var minDelay = TimeSpan.FromSeconds(30);

            while (true)
            {
                var sourceDocument = _sourceStats.Items.FindAndModify(Query<SourceStats>.LT(x => x.Fetched, now.Add(-minDelay)),
                    SortBy<SourceStats>.Ascending(x => x.Fetched),
                    Update<SourceStats>.Inc(x => x.Count, 1).Set(x => x.Fetched, now))
                    .GetModifiedDocumentAs<SourceStats>();
                if (sourceDocument != null)
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    List<int> subjectIds = null;
                    try
                    {
                        ResetRank(sourceDocument.SourceId);
                        subjectIds = GetSubjects(sourceDocument.SourceId);
                        var analyzer = new SourceAnalyzer(sourceDocument.SourceId, subjectIds);
                        analyzer.Run();
                    }
                    catch (Exception e)
                    {
                        //_sourceStats.Items.Update(Query<SourceStats>.EQ(x => x.SourceId, sourceDocument.SourceId),
                        //    Update<SourceStats>.Inc(x => x.Count, -1).Set(x => x.Fetched, sourceDocument.Fetched));
               
                    }
                    finally
                    {
                        stopWatch.Stop();
                        //Save also arrours occured during fetching and resource id
                        SaveStats(sourceDocument, subjectIds, stopWatch.ElapsedMilliseconds);
                    }
                }
            }
        }

        private void SaveStats(SourceStats sourceDocument, List<int> subjectIds, long elapsedMilliseconds)
        {
            _stats.Items.Insert(new FetchingStats
            {
                Id = ObjectId.GenerateNewId().ToString(),
                SourceId = sourceDocument.SourceId,
                Subjects = subjectIds,
              //  Rank = sourceDocument.Rank,
                Processed = DateTime.UtcNow,
                Elapsed = elapsedMilliseconds
            });
        }

        private List<int> GetSubjects(int id)
        {
            return _sources.Items.Find(Query<SourceDocument>.EQ(x=>x.SourceId, id)).SetFields("SubjectId").Select(x=> x.SubjectId).ToList();
        }

        private void ResetRank(int sourceId)
        {
            _sources.Items.Update(Query<SourceDocument>.EQ(x => x.SourceId, sourceId),
                Update<SourceDocument>.Set(x => x.Rank, 0));
        }
    }
}