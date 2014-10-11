﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Views;
using ISeeYou.ViewServices;
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
            container.GetInstance<SourcesViewService>().Save(new SourceDocument { Id = 2409833, Rank = 22, SubjectId = 28232817 });
            var token = container.GetInstance<SubjectViewService>().Items.FindOne(Query<SubjectView>.NE(x => x.Token, null)).Token;
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
