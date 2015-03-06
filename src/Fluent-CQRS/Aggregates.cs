using System;
using System.Collections.Generic;
using System.Linq;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    public sealed class Aggregates
    {
        private readonly IStoreAndRetrieveEvents _eventStore;

        public Aggregates(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public ProvideAggregate<TAggregate> Provide<TAggregate>() where TAggregate : Aggregate
        {
            return new ProvideAggregate<TAggregate>(_eventStore, this);
        }

        public void SaveChangesBy(Aggregate aggregate)
        {
            var newEvents = aggregate
                .Changes
                .ToList();

            var aggregateId = aggregate.Id;

            newEvents.ForEach(eventMessage => _eventStore.StoreFor(aggregateId, eventMessage));
        }

        public Action<IEnumerable<IAmAnEventMessage>> PublishNewState { get; set; }  
    }
}