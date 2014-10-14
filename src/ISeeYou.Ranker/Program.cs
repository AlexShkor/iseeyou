using System;
using ISeeYou.ViewServices;
using StructureMap;

namespace ISeeYou.Ranker
{
    class Program
    {
        static int Main(string[] args)
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
                        container.GetInstance<VkRanker>().UpdateRankedProfiles(subjectView.Id);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}
