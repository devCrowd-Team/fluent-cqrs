using System.Collections.Generic;

namespace Fluent_CQRS.Tests.Infrastructure
{
    class AlternativeTestEventHandler : IHandleEvents
    {
        public List<IAmAnEventMessage> RecievedEvents = new List<IAmAnEventMessage>();

        public void Receive(IEnumerable<IAmAnEventMessage> events)
        {
            RecievedEvents.AddRange(events);
        }
    }
}