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

        public void StoreFor(String aggregateId, ICarryEventInformation eventInformation)
        {
            _events.Add(new EventBag
            {
                AggregateId = aggregateId,
                TypeOfData = eventInformation.GetType(),
                TimeStamp = DateTime.UtcNow,
                Information = eventInformation
            });
        }

        public IEnumerable<ICarryEventInformation> RetrieveFor(String aggregateId)
        {
            return _events
                .Where(eventMessage => eventMessage.AggregateId.Equals(aggregateId))
                .Select(eventBag => eventBag.Information)
                .ToArray();
        }
    }
}