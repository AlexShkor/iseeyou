using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Fetching;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Workers;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using StructureMap;namespace ISeeYou.Fetcher
{
    class Program
    {
        static void Main(string[] args)
        {
            PhotoWorker.Start();
        }
    }
}
