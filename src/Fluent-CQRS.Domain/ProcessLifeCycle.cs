using Fluent_CQRS.Extensions;
using Fluent_CQRS.Fluentation;
using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
    internal class ProcessLifeCycle<TProcessManager> : 
        IProvideAProcessManager<TProcessManager>, 
        ITellEventsToProcessManagers<TProcessManager> where TProcessManager:ProcessManager
    {
     
        readonly IStoreAndRetrieveEvents _eventStore;
        private Action<IEnumerable<IAmACommandMessage>> _publishMethod;
        static TProcessManager _processManager;

        public ProcessLifeCycle(IStoreAndRetrieveEvents eventStore, Action<IEnumerable<IAmACommandMessage>> publishMethod )
        {
            _eventStore = eventStore;
            _publishMethod = publishMethod;          
        }

        public ITellEventsToProcessManagers<TProcessManager> With(CorrelationId id)
        {
            var processEvents = _eventStore.RetrieveFor(id.Value);

            _processManager = FromHistory(id.Value, processEvents);

            return this;
        }

        private static TProcessManager FromHistory(string processId, IEnumerable<IAmAnEventMessage> history)
        {
            var processManagerAsObject =  Activator.CreateInstance(typeof(TProcessManager), processId, history);

            return ((TProcessManager) processManagerAsObject);
        }

        public ExecutionResult Handle(IAmAnEventMessage @event)
        {
            var executionResult = InvokeTransition(@event);

            executionResult = StoreChanges(executionResult);

            executionResult = PublishCommands(executionResult);

            return executionResult;
        }
       

        private ExecutionResult PublishCommands(ExecutionResult executionResult)
        {
            if (executionResult.HasErrors()) return executionResult;

            _publishMethod(_processManager.Commands);
            _processManager.Commands.Clear();
         
            executionResult.Published = true;
            return executionResult;
        }

        private ExecutionResult StoreChanges(ExecutionResult executionResult)
        {
            if (executionResult.HasErrors()) return executionResult;

            var processId = _processManager.Id;

            foreach (var eventMessage in _processManager.Changes)
            {
                _eventStore.StoreFor<TProcessManager>(processId, eventMessage);
            }

            _processManager.Changes.Clear();
            executionResult.Saved = true;

            return executionResult;
        }

        private ExecutionResult InvokeTransition(IAmAnEventMessage msg) 
        {
            var executionResult = new ExecutionResult();

            _processManager.Changes.Add(msg);
            _processManager.Tell(msg);

            executionResult.Executed = true;

            return executionResult;
        }
    }
}