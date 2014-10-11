using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly UsersViewService _users;

        public SourceFetcher(SubjectViewService subjects, SourcesViewService sources, UsersViewService users)
        {
            _subjects = subjects;
            _sources = sources;
            _users = users;
        }

        public void Run()
        {
            var cursor = _sources.Items.Find(Query<SourceDocument>.NE(x=> x.Rank, 0)).SetSortOrder(SortBy<SourceDocument>.Descending(x => x.Rank));
            foreach (var sourceDocument in cursor)
            {
                try
                {
                    var subjectIds = GetSubjects(sourceDocument.Id);
                    var analyzer = new SourceAnalyzer(sourceDocument.Id, subjectIds);
                    analyzer.Run();
                    ResetRank(sourceDocument.Id);
                }
                catch (Exception e)
                {
                    
                }
            }
        }

        private List<int> GetSubjects(int id)
        {
            return _sources.Items.Find(Query.EQ("_id", id)).SetFields("SubjectId").Select(x=> x.SubjectId).ToList();
        }

        private void ResetRank(int sourceId)
        {
            _sources.Items.Update(Query<SourceDocument>.EQ(x => x.Id, sourceId),
                Update<SourceDocument>.Set(x => x.Rank, 0).Inc(x => x.Calls, 1));
        }

        //private List<int> GetSubjects(int sourceId)
        //{
        //    return _subjects.Items.Find(Query<SubjectView>.ElemMatch(x => x.Sources,
        //            builder => builder.EQ(x => x, sourceId))).SetFields("Id").Select(x => x.Id).ToList();
        //}
    }
}