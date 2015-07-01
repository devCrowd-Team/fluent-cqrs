using System;
using System.Collections.Generic;
using System.Linq;
using Fluent_CQRS.Extensions;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    internal class AggregateLifeCycle<TAggregate> : 
        IProvideAnAggregate<TAggregate>, 
        IInvokeActionsOnAggregates<TAggregate> where TAggregate : Aggregate
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

            _aggregate = (TAggregate)aggregateAsObject;
            return _aggregate;
        }

        public IInvokeActionsOnAggregates<TAggregate> With(IAmACommandMessage command) 
        {
            var aggregateEvents = _eventStore.RetrieveFor(command.Id);

            var aggregateInstance = AggregateInstance(command);

            aggregateInstance.History = aggregateEvents;

            return this;
        }

        public ExecutionResult<ExecutionFlags> Do(Action<TAggregate> doAction)
        {
            return 
                InvokeAggregateMethod(doAction)
                .ThenTry(StoreChanges)
                .ThenTry(PublishChanges);
        }

        private ExecutionResult<ExecutionFlags> InvokeAggregateMethod(Action<TAggregate> doAction)
        {
            return ExecutionResult.Try(() =>
            {
                doAction.Invoke(_aggregate);
                return new ExecutionFlags {WasExecuted = true};
            });
        }

        private ExecutionFlags StoreChanges(ExecutionFlags flags)
        {
            var aggregateId = _aggregate.Id;

            _aggregate
                .Changes
                .ToList()
                .ForEach(eventMessage =>
                    _eventStore.StoreFor(aggregateId, eventMessage));

            return flags.Saved();
        }

        private ExecutionFlags PublishChanges(ExecutionFlags flags)
        {
            if (_publishMethod.IsNotDefined())
                throw new MissingEventsPublishingTarget();

            _publishMethod(_aggregate.Changes);
            _aggregate.Changes.Clear();

            return flags.Published();
        }
    }
}