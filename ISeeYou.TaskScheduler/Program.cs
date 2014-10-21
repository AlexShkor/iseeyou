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
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Driver.Builders;
using StructureMap;

namespace ISeeYou.TaskScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "dubina.by";
            var user = "spypie";
            var pwd = "GM9SGQoLngSaJYZ";
            var fromWebEx = "spypie_from_web";

            
            while (true)
            {
                // just for example
                using (var publisher = new RabbitMqPublisher(host, user, pwd, fromWebEx))
                {
                    publisher.Publish(new SubjectAddedEvent());
                }
            }
        }
    }
}
