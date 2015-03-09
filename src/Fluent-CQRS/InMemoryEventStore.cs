using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public sealed class InMemoryEventStore : IStoreAndRetrieveEvents
    {
        private readonly IList<EventBag> _events;

        public InMemoryEventStore()
        {
            _events = new List<EventBag>();
        }

        public void StoreFor(String aggregateId, IAmAnEventMessage eventMessage)
        {
            _events.Add(new EventBag
            {
                AggregateId = aggregateId,
                TypeOfEvent = eventMessage.GetType(),
                TimeStamp = DateTime.UtcNow,
                Event = eventMessage
            });
        }

        public IEnumerable<IAmAnEventMessage> RetrieveFor(String aggregateId)
        {
            return _events
                .Where(eventMessage => eventMessage.AggregateId.Equals(aggregateId))
                .Select(eventBag => eventBag.Event)
                .ToArray();
        }
    }
}