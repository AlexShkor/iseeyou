using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using StructureMap;
using VkAPIAsync;
using IContainer = System.ComponentModel.IContainer;

namespace ISeeYou.Fetcher
{
    class Program
    {
        static void Main(string[] args)
        {
            StructureMap.IContainer container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);

            #region Get token from app
            //_app = _container.GetInstance<AppsViewService>().Items.FindOne(Query<AppView>.EQ(x => x.IsAvailable, true));
            //if (_app == null)
            //{
            //    return;
            //}
            //_container.GetInstance<AppsViewService>()
            //    .Items.Update(Query<AppView>.EQ(x => x.Id, _app.Id), Update<AppView>.Set(x => x.IsAvailable, false));
            //var token = _app.Token;
            //var token = container.GetInstance<SubjectViewService>().Items.FindOne(Query<SubjectView>.NE(x => x.Token, null)).Token;
            #endregion

            var token = container.GetInstance<SiteSettings>().FetcherToken;
            VkAPI.AccessToken = token;
            while (true)
            {
                try
                {
                    container.GetInstance<SourceFetcher>().Run().Wait();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
