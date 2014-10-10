using System.Collections.Generic;
using System.Linq;
using ISeeYou.Platform.Domain.Interfaces;
using ISeeYou.Platform.Domain.Transitions;

namespace ISeeYou.Platform.Upgrade
{
    public abstract class EventUpgrader : IUpgrader
    {
        public abstract int Number { get; }

        public bool IsEnabled { get { return true; } }

        /// <summary>
        /// This method will be called for each event in EventStore.
        /// Order will be according to Timestamp property of transitions, ascending.
        /// </summary>
        protected abstract IEnumerable<IEvent> Upgrade(IEvent evnt);

        public IEnumerable<Transition> Upgrade(Transition transition)
        {
            var events = new List<IEvent>();

            foreach (var evnt in transition.Events)
            {
                var upgraded = Upgrade((IEvent)evnt.Data);
                events.AddRange(upgraded);
            }

            if (events.Any())
            {
                transition.Events.Clear();
                transition.Events.AddRange(events.Select(e => new TransitionEvent(e.GetType().AssemblyQualifiedName, e)));
                yield return transition;
            }
        }
    }
}