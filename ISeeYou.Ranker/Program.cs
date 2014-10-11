using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ISeeYou.Databases;
using ISeeYou.Documents;
using ISeeYou.Helpers;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.VkRanking;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using StructureMap;
using VkAPIAsync;

namespace ISeeYou.Ranker
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Starting");
            StructureMap.IContainer container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var subjects = container.GetInstance<SubjectViewService>();
            var appId = container.GetInstance<SiteSettings>().FetcherToken;
            while (true)
            {
                var all = subjects.GetAll();
                foreach (var subjectView in all)
                {
                    try
                    {
                        Console.WriteLine("Precessing Subject " + subjectView.Id);
                        Thread.Sleep(400);
                        var ranker = container.GetInstance<VkRanker>();
                        ranker.Authorize(int.Parse(appId),"ineoi@tut.by","paralects1");
                        ranker.UpdateRankedProfiles(subjectView.Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error!");
                    }
                }
            }
        }
    }
}
