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

        public IInvokeActionsOnAggregates<TAggregate> With(IAmACommandMessage command)
        {
            var aggregateId = new AggregateId(command.Id);

            return With(aggregateId);
        }

        public IInvokeActionsOnAggregates<TAggregate> With(AggregateId id)
        {
            var aggregateEvents = _eventStore.RetrieveFor(id.Value);

            _aggregate = BuildAggregateFromHistory(id.Value, aggregateEvents);

            return this;
        }

        public void Do(Action<TAggregate> doAction)
        {
            var executionResult = InvokeAggregateMethod(doAction);

            executionResult = StoreChanges(executionResult);

            executionResult = PublishChanges(executionResult);

        }

        public ExecutionResult Try(Action<TAggregate> doAction)
        {
            var executionResult = TryInvokeAggregateMethod(doAction);

            executionResult = TryStoreChanges(executionResult);

            executionResult = TryPublishChanges(executionResult);

            return executionResult;
        }

        private static TAggregate BuildAggregateFromHistory(string aggregateId, IEnumerable<IAmAnEventMessage> history)
        {
            var aggregateAsObject = Activator.CreateInstance(typeof(TAggregate), aggregateId, history);

            return ((TAggregate)aggregateAsObject);
        }

        private ExecutionResult TryInvokeAggregateMethod(Action<TAggregate> doAction)
        {
            var executionResult = new ExecutionResult();

            try
            {
                executionResult = InvokeAggregateMethod(doAction);

            }
            catch (Fault fault)
            {
                executionResult.Fault = fault;
            }
            catch (Exception ex)
            {

                executionResult.Exception = ex;
            }

            return executionResult;
        }

        private ExecutionResult InvokeAggregateMethod(Action<TAggregate> doAction)
        {
            var executionResult = new ExecutionResult();

            doAction.Invoke(_aggregate);

            executionResult.Executed = true;

            return executionResult;
        }

        private ExecutionResult TryStoreChanges(ExecutionResult executionResult)
        {
            try
            {
                executionResult = StoreChanges(executionResult);
            }
            catch (Exception ex)
            {
                executionResult.Exception = ex;
                return executionResult;
            }

            return executionResult;
        }

        private ExecutionResult StoreChanges(ExecutionResult executionResult)
        {
            if (executionResult.HasErrors()) return executionResult;

            var aggregateId = _aggregate.Id;

            foreach (var eventMessage in _aggregate.Changes)
            {
                _eventStore.StoreFor<TAggregate>(aggregateId, eventMessage);
            }

            executionResult.Saved = true;

            return executionResult;
        }

        private ExecutionResult TryPublishChanges(ExecutionResult executionResult)
        {
            try
            {
                executionResult = PublishChanges(executionResult);
            }
            catch (Exception ex)
            {
                executionResult.Exception = ex;
                return executionResult;
            }

            return executionResult;
        }

        private ExecutionResult PublishChanges(ExecutionResult executionResult)
        {
            if (executionResult.HasErrors()) return executionResult;

            _publishMethod(_aggregate.Changes.Concat(_aggregate.EventsToReplay));


            _aggregate.Changes.Clear();
            _aggregate.EventsToReplay.Clear();

            executionResult.Published = true;
            return executionResult;
        }
    }
}