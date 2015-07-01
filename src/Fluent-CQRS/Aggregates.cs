using System;
using System.Collections.Generic;
using Fluent_CQRS.Extensions;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    public sealed class Aggregates : IObservable<ICarryEventInformation>
    {
        private readonly IStoreAndRetrieveEvents _eventStore;
        private readonly Subject<IEnumerable<ICarryEventInformation>> _publishedEvents;

        public static Aggregates CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new Aggregates(eventStore);
        }

        internal Aggregates(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
            _publishedEvents = new Subject<IEnumerable<ICarryEventInformation>>();
        }

        public IProvideAnAggregate<TAggregate> Provide<TAggregate>() where TAggregate : Aggregate
        {
            return new AggregateLifeCycle<TAggregate>(_eventStore, _publishedEvents.OnNext);
        }

        public IDisposable Subscribe(IObserver<ICarryEventInformation> observer)
        {
            return _publishedEvents.Flatten().Subscribe(observer);
        }
    }
}