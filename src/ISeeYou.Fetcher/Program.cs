using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
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
            for (int i = 0; i < 5; i++)
            {
                container.GetInstance<SourceFetcher>().Run();
            }
        }
    }
}
