using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Ranking;
using ISeeYou.ViewServices;
using StructureMap;

namespace ISeeYou.PhotoRanker
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var subjects = container.GetInstance<SubjectViewService>();
            while (true)
            {
                var all = subjects.GetAll();
                foreach (var subjectView in all)
                {
                    try
                    {
                        container.GetInstance<SubjectRanker>().UpdateRankedProfiles(subjectView.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
