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
            Console.WriteLine("Starting");
            StructureMap.IContainer container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var subjects = container.GetInstance<SubjectViewService>();
            var appId = container.GetInstance<SiteSettings>().FetcherToken;
            var application = container.GetInstance<AppsViewService>()
                .GetById(appId);
            var token = application != null ? application.Token : null;
            if (token == null)
            {
                Console.WriteLine("Exiting");
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
                        Console.WriteLine("Precessing Subject " + subjectView.Id);
                        Task.Delay(400).Wait();
                        container.GetInstance<VkRanker>().UpdateRankedProfiles(subjectView.Id);
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine("AggregateException: " + string.Join(";", e.InnerExceptions.Select(x => x.Message).ToArray()));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
        }
    }
}
