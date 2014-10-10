using System.Collections.Generic;
using ISeeYou.Platform.Dispatching.Interfaces;
using ISeeYou.Platform.Domain.Interfaces;

namespace ISeeYou.Platform.Domain.EventBus
{
    public class DispatcherEventBus : IEventBus
    {
        private readonly IDispatcher _dispatcher;

        public DispatcherEventBus(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Publish(IEvent eventMessage)
        {
            _dispatcher.Dispatch(eventMessage);
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            foreach (var evnt in eventMessages)
                _dispatcher.Dispatch(evnt);
        }
    }
}