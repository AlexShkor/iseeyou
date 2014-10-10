using ISeeYou.Documents;
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
                var analyzer = new SourceAnalyzer(sourceDocument.Id,subjectIds );
                analyzer.Run();
            }
        }

        private string[] GetSubjects(string sourceId)
        {
            throw new System.NotImplementedException();
        }
    }
}