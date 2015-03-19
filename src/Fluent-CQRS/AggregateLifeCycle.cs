using System;
using System.Collections.Generic;
using System.Linq;
using Fluent_CQRS.Extensions;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    internal class AggregateLifeCycle<TAggregate> : 
        IProvideAnAggregate<TAggregate>, 
        IInvokeActionsOnAggregates<TAggregate>,
        ISaveAggregates,
        IPublishNewState where TAggregate : Aggregate
    {
        readonly IStoreAndRetrieveEvents _eventStore;
        readonly Action<IEnumerable<IAmAnEventMessage>> _publishMethod;
        static TAggregate _aggregate;

        public AggregateLifeCycle(IStoreAndRetrieveEvents eventStore, Action<IEnumerable<IAmAnEventMessage>> publishMethod)
        {
            _eventStore = eventStore;
            _publishMethod = publishMethod;
        }

        private TAggregate AggregateInstance(IAmACommandMessage command)
        {
            var aggregateAsObject = Activator.CreateInstance(typeof(TAggregate), command.Id);

            _aggregate = ((TAggregate)aggregateAsObject);
            return _aggregate;
        }

        public IInvokeActionsOnAggregates<TAggregate> With(IAmACommandMessage command) 
        {
            var aggregateEvents = _eventStore.RetrieveFor(command.Id);

            var aggregateInstance = AggregateInstance(command);

            aggregateInstance.History = aggregateEvents;

            return this;
        }

        public ISaveAggregates Do(Action<TAggregate> doAction)
        {
            doAction.Invoke(_aggregate);

            return this;
        }

        public IPublishNewState FinallySaveIt()
        {
            var aggregateId = _aggregate.Id;

            _aggregate
                .Changes
                .ToList()
                .ForEach(eventMessage =>
                    _eventStore.StoreFor(aggregateId, eventMessage));

            return this;
        }

        public void AndPublishNewState()
        {
            if (_publishMethod.IsNotDefined())
                throw new MissingEventsPublishingTarget();

            _publishMethod(_aggregate.Changes);
            _aggregate.Changes.Clear();
        }
    }
}