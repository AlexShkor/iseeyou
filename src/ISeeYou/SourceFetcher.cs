using System.Collections.Generic;
using System.Linq;
using ISeeYou.Documents;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using MongoDB.Driver.Builders;

namespace ISeeYou
{
    public class SourceFetcher
    {
        private readonly SubjectViewService _subjects;
        private readonly SourcesViewService _sources;

        public SourceFetcher(SubjectViewService subjects, SourcesViewService sources)
        {
            _subjects = subjects;
            _sources = sources;
        }

        public void Run()
        {
            var cursor = _sources.Items.FindAll().SetSortOrder(SortBy<SourceDocument>.Descending(x => x.Rank));
            foreach (var sourceDocument in cursor)
            {
                var subjectIds = GetSubjects(sourceDocument.Id);
                var analyzer = new SourceAnalyzer(sourceDocument.Id,subjectIds);
                analyzer.Run();
                ResetRank(sourceDocument.Id);
            }
        }

        private void ResetRank(int sourceId)
        {
            _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, sourceId),
                Update<SourceDocument>.Set(x => x.Rank, 0));
        }

        private List<int> GetSubjects(int sourceId)
        {
            return
                _subjects.Items.Find(Query<SubjectView>.ElemMatch(x => x.Sources,
                    builder => builder.EQ(x => x, sourceId))).SetFields("Id").Select(x => x.Id).ToList();
        }
    }
}