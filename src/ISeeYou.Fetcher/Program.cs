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
        static int Main(string[] args)
        {
            StructureMap.IContainer container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);

            while (true)
            {
                try
                {
                    container.GetInstance<SourceFetcher>().Run();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
