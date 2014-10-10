using System.Collections.Generic;
using ISeeYou.Platform.Domain.Interfaces;

namespace ISeeYou.Platform.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}