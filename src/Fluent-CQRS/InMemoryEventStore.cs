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

        public void StoreFor<TAggregate>(string aggregateId, IAmAnEventMessage eventMessage) where TAggregate : Aggregate
        {
            _events.Add(new EventBag
            {
                AggregateId = aggregateId,
                TypeOfAggregate = typeof(TAggregate),
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

        public IEnumerable<IAmAnEventMessage> RetrieveFor<TAggregate>(string aggregateId) 
            where TAggregate : Aggregate
        {
            return _events
                .Where(eventMessage =>
                    eventMessage.AggregateId.Equals(aggregateId)
                    && eventMessage.TypeOfAggregate == typeof(TAggregate))
                .Select(eventBag => eventBag.Event)
                .ToArray();
        }
    }
}