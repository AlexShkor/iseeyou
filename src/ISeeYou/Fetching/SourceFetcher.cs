using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.ViewServices;
using MongoDB.Driver.Builders;

namespace ISeeYou.Fetching
{
    public class SourceFetcher
    {
        private readonly SourcesViewService _sources;
        private readonly FetchingStatsService _stats;

        public SourceFetcher(SourcesViewService sources, FetchingStatsService stats)
        {
            _sources = sources;
            _stats = stats;
        }

        public void Run()
        {
            var cursor = _sources.Items.Find(Query<SourceDocument>.NE(x=> x.Rank, 0)).SetSortOrder(SortBy<SourceDocument>.Descending(x => x.Rank));
            foreach (var sourceDocument in cursor)
            {
                try
                {
                    ResetRank(sourceDocument.SourceId);
                    var subjectIds = GetSubjects(sourceDocument.SourceId);
                    var analyzer = new SourceAnalyzer(sourceDocument.SourceId, subjectIds);
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    analyzer.Run();
                    stopWatch.Stop();
                    SaveStats(sourceDocument, subjectIds, stopWatch.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    
                }
            }
        }

        private void SaveStats(SourceDocument sourceDocument, List<int> subjectIds, long elapsedMilliseconds)
        {
            _stats.Save(new FetchingStats
            {
                SourceId = sourceDocument.SourceId,
                Subjects = subjectIds,
                Rank = sourceDocument.Rank,
                Processed = DateTime.Now,
                Elapsed = elapsedMilliseconds
            });
        }

        private List<int> GetSubjects(int id)
        {
            return _sources.Items.Find(Query.EQ("_id", id)).SetFields("SubjectId").Select(x=> x.SubjectId).ToList();
        }

        private void ResetRank(int sourceId)
        {
            _sources.Items.Update(Query<SourceDocument>.EQ(x => x.SourceId, sourceId),
                Update<SourceDocument>.Set(x => x.Rank, 0));
        }

        //private List<int> GetSubjects(int sourceId)
        //{
        //    return _subjects.Items.Find(Query<SubjectView>.ElemMatch(x => x.Sources,
        //            builder => builder.EQ(x => x, sourceId))).SetFields("Id").Select(x => x.Id).ToList();
        //}
    }
}