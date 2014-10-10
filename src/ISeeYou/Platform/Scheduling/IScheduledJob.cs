using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace ISeeYou.Platform.Scheduling
{
    public interface IScheduledJob : IJob
    {
        JobDetailImpl ConfigureJob();

        SimpleTriggerImpl ConfigureTrigger();

        bool IsEnabled { get; }
    }
}
