using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Schedulers;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using StructureMap;

namespace ISeeYou.SourceScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            SourcesScheduler.StartNew(container);
        }
    }
}
