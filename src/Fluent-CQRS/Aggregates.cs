using System.Collections.Generic;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    public sealed class Aggregates
    {
        private readonly IStoreAndRetrieveEvents _eventStore;
        private readonly EventHandlers _eventHandlers;

        public static Aggregates CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new Aggregates(eventStore);
        }

        internal Aggregates(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
            _eventHandlers = new EventHandlers();
        }

        public IProvideAnAggregate<TAggregate> Provide<TAggregate>() where TAggregate : Aggregate
        {
            return new AggregateLifeCycle<TAggregate>(_eventStore, NewStateCallback);
        }

        public IConcatenateEventHandler PublishNewStateTo(object eventHandler)
        {
            _eventHandlers.Add(eventHandler);

            return _eventHandlers;
        }

        private void NewStateCallback(IEnumerable<IAmAnEventMessage> events)
        {
            _eventHandlers.Receive(events);
        }
    }
}