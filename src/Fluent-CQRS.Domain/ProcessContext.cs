using Fluent_CQRS.Fluentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS
{
    public class ProcessContext
    {
        readonly IStoreAndRetrieveEvents _eventStore;
        readonly Handlers<IAmACommandMessage> _commandHandlers;
        
        internal ProcessContext(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
            _commandHandlers = new Handlers<IAmACommandMessage>();
        }

        public static ProcessContext CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new ProcessContext(eventStore);
        }

        public IProvideAProcessManager<TSaga> Provide<TSaga>() where TSaga :ProcessManager
        {
            var typeOfSaga = typeof(TSaga);
            return new ProcessLifeCycle<TSaga>(_eventStore, TypedNewStateCallBack(typeOfSaga));
        }

        public IConcatenateHandlers<IAmACommandMessage> DispatchCommandsTo(IHandle<IEnumerable<IAmACommandMessage>> commandHandler)
        {
            _commandHandlers.Add(commandHandler);
            return _commandHandlers;
        }

        public IConcatenateHandlers<IAmACommandMessage> DispatchCommandsTo(IHandle<IAmACommandMessage> commandHandler)
        {
            _commandHandlers.Add(commandHandler);
            return _commandHandlers;
        }


        void DispatchMessages(IEnumerable<IAmACommandMessage> commands)
        {
            _commandHandlers.Receive(commands);
        }

        private Action<IEnumerable<IAmACommandMessage>> TypedNewStateCallBack(Type typeOfSaga)
        {
            var focusedCallBack = new Action<IEnumerable<IAmACommandMessage>>(commands =>
            {
                 DispatchMessages(commands);
            });
            return focusedCallBack;
        }
    }
}
