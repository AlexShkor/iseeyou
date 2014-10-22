using System;
using System.Linq;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using ISeeYou.Workers;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.Worker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PhotoWorker.Start();
        }
    }
}
