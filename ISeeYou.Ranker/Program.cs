using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.VkRanking;
using MongoDB.Driver.Builders;
using StructureMap;
using VkAPIAsync;

namespace ISeeYou.Ranker
{
    class Program
    {
        static void Main(string[] args)
        {
            StructureMap.IContainer container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var subjects = container.GetInstance<SubjectViewService>();
            var token = container.GetInstance<SiteSettings>().FetcherToken;
            VkAPI.AccessToken = token;
            while (true)
            {
                var all = subjects.GetAll();
                foreach (var subjectView in all)
                {
                    VkAPI.AccessToken = subjectView.Token ?? VkAPI.AccessToken;
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
