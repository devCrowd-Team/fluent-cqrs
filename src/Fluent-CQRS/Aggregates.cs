using System;
using System.Collections.Generic;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    public sealed class Aggregates
    {
        private readonly IStoreAndRetrieveEvents _eventStore;

        public static Aggregates CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new Aggregates(eventStore);
        }

        internal Aggregates(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public IProvideAnAggregate<TAggregate> Provide<TAggregate>() where TAggregate : Aggregate
        {
            return new AggregateLifeCycle<TAggregate>(_eventStore, PublishNewState);
        }

        public Action<IEnumerable<IAmAnEventMessage>> PublishNewState { get; set; }
    }
}