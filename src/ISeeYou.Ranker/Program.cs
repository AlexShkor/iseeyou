using System;
using System.Threading.Tasks;
using ISeeYou.Ranking;
using ISeeYou.Schedulers;
using ISeeYou.ViewServices;
using StructureMap;

namespace ISeeYou.Ranker
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            PhotoScheduler.StartNew(container);
        }
    }
}
