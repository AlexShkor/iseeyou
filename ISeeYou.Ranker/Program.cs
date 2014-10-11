﻿using System;
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
        static int Main(string[] args)
        {
            StructureMap.IContainer container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var subjects = container.GetInstance<SubjectViewService>();
            var appId = "4584967";
            var application = container.GetInstance<AppsViewService>()
                .Items.FindOne(Query<AppView>.EQ(x => x.Id, appId));
            var token = application != null ? application.Token : null;
            if (token == null)
            {
                return 1;
            }

            VkAPI.AccessToken = token;
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
