using System;
using System.Threading;
using ISeeYou.MQ;
using ISeeYou.MQ.Events;
using StructureMap;

namespace ISeeYou.UniversalWorker
{
    class Program
    {
        static void Main(string[] args)
        {

            // temporary placing this here

            var host = "dubina.by";
            var user = "spypie";
            var pwd = "GM9SGQoLngSaJYZ";
            var fromWebEx = "spypie_from_web";

            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            //new Bootstrapper().ConfigureMongoDb(container);

            var subjectAddedConsumer = new RabbitMqConsumer<SubjectAddedEvent>(host, user, pwd, fromWebEx);

            subjectAddedConsumer.On += subject =>
            {
                Console.WriteLine("Msg received: {0}", subject);
                // route message here
            };

            subjectAddedConsumer.Start();
            
            Thread.Sleep(-1);
        }
    }
}
