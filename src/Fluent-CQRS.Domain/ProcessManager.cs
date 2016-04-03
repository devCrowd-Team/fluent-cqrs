using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public abstract class ProcessManager : Aggregate
    {
        class EventHandler : Handler<IAmAnEventMessage>
        {
            public new void Receive<T>(Action<T> handle)
            {
                base.Receive(handle);
            }
        }

        EventHandler eventHandler;

        public ProcessManager(string id, IEnumerable<IAmAnEventMessage> history) : base(id, history)
        {
            eventHandler = new EventHandler();
            Commands = new List<IAmACommandMessage>();
            ReplayHistory = () =>  ReplayHistoryInternal(history);
        }

        protected internal IList<IAmACommandMessage> Commands { get; private set; }

        protected Action ReplayHistory { get; private set; }

        private void ReplayHistoryInternal(IEnumerable<IAmAnEventMessage> history)
        {
            foreach (var @event in history.ToList())
                Tell(@event);
            Changes.Clear();
            Commands.Clear();
        }

        protected void Receive<T>(Action<T> handle)
        {
            eventHandler.Receive(handle);
        }

        public void Tell(IAmAnEventMessage @event)
        {
            eventHandler.Tell(@event);
        }
    }
}
