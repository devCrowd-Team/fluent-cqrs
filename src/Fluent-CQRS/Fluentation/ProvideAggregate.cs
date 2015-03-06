using System;
using System.Linq;

namespace Fluent_CQRS.Fluentation
{
    public sealed class ProvideAggregate<TAggregate> where TAggregate : Aggregate
    {
        private readonly IStoreAndRetrieveEvents _eventStore;
        private readonly Aggregates _aggregates;

        public ProvideAggregate(IStoreAndRetrieveEvents eventStore, Aggregates aggregates)
        {
            _eventStore = eventStore;
            _aggregates = aggregates;
        }

        public DoActionOnAggregate<TAggregate> With(IAmACommandMessage command)
        {
            var aggregateEvents = _eventStore.RetrieveFor(command.Id);

            var aggregateInstance = AggregateInstance(command);

            aggregateInstance.History = aggregateEvents;

            return new DoActionOnAggregate<TAggregate>(aggregateInstance, _aggregates);
        }

        static TAggregate AggregateInstance(IAmACommandMessage command)
        {
            var aggregateAsObject = Activator.CreateInstance(typeof (TAggregate), command.Id);

            var aggregateInstance = ((TAggregate) aggregateAsObject);
            return aggregateInstance;
        }
    }
}