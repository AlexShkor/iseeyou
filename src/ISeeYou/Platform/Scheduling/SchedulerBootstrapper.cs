using System.Reflection;
using ISeeYou.Platform.StructureMap;
using Microsoft.Practices.ServiceLocation;
using Quartz.Spi;
using StructureMap;

namespace ISeeYou.Platform.Scheduling
{
    public class SchedulerBootstrapper
    {
        public void Configure(IContainer container, Assembly jobsAssembly)
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));

            container.Configure(config => config.For<IJobFactory>().Use(new IoCJobFactory(container)));

            container.Configure(config => config.Scan(scanner =>
            {
                scanner.Assembly(jobsAssembly);
                scanner.AddAllTypesOf<IScheduledJob>();
            }));
        }
    }
}
