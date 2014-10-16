using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
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

            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            //new Bootstrapper().ConfigureMongoDb(container);

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
