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

		public AggregateLifeCycle (IStoreAndRetrieveEvents eventStore, Action<IEnumerable<IAmAnEventMessage>> publishMethod)
		{
			_eventStore = eventStore;
			_publishMethod = publishMethod;
		}

		private TAggregate AggregateInstance (IAmACommandMessage command)
		{
			var aggregateAsObject = Activator.CreateInstance (typeof(TAggregate), command.Id);

			_aggregate = ((TAggregate)aggregateAsObject);
			return _aggregate;
		}

		public IInvokeActionsOnAggregates<TAggregate> With (IAmACommandMessage command)
		{
			var aggregateEvents = _eventStore.RetrieveFor (command.Id);

			var aggregateInstance = AggregateInstance (command);

			aggregateInstance.History = aggregateEvents;

			return this;
		}

		public ExecutionResult Do (Action<TAggregate> doAction)
		{
			var executionResult = InvokeAggregateMethod (doAction);

			executionResult = StoreChanges (executionResult);

			executionResult = PublishChanges (executionResult);

			return executionResult;
		}

		private ExecutionResult InvokeAggregateMethod (Action<TAggregate> doAction)
		{
			var executionResult = new ExecutionResult ();

			try {
				doAction.Invoke (_aggregate);

				executionResult.Executed = true;
			} catch (Exception ex) {
				executionResult.Error = ex;
				return executionResult;
			}

			return executionResult;
		}

		private ExecutionResult StoreChanges (ExecutionResult executionResult)
		{
			try {
				var aggregateId = _aggregate.Id;

			    foreach (var eventMessage in _aggregate.Changes)
			    {
			        _eventStore.StoreFor(aggregateId, eventMessage);
			    }

				executionResult.Saved = true;
			} catch (Exception ex) {
				executionResult.Error = ex;
				return executionResult;
			}

			return executionResult;
		}

		private ExecutionResult PublishChanges (ExecutionResult executionResult)
		{
			try {
				_publishMethod (_aggregate.Changes);
			    _publishMethod (_aggregate.EventsToReplay);

				_aggregate.Changes.Clear ();
                _aggregate.EventsToReplay.Clear();
                
				executionResult.Published = true;
			} catch (Exception ex) {
				executionResult.Error = ex;
				return executionResult;
			}

			return executionResult;
		}
	}
}