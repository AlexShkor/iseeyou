using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Documents;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using StructureMap;

namespace ISeeYou.SourceWorker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Workers.SourceWorker.Start();
        }
    }
}
