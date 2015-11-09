using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Aggregate
    {
        public Aggregate(string id, IEnumerable<IAmAnEventMessage> history)
        {
            Changes = new List<IAmAnEventMessage>();
            History = new List<IAmAnEventMessage>(history);
            EventsToReplay = new List<IAmAnEventMessage>();

            Id = id;
        }

        private IEnumerable<IAmAnEventMessage> History { get; set; }

        protected internal IList<IAmAnEventMessage> Changes { get; private set; }

        protected internal IList<IAmAnEventMessage> EventsToReplay { get; private set; }

        protected internal IEnumerable<T> MessagesOfType<T>() where T : IAmAnEventMessage
        {
            return History.Concat(Changes).OfType<T>();
        }

        protected internal Fold<TState> InitializedAs<TState>()
        {
            return new Fold<TState>(History.Concat(Changes));
        }

        protected internal Fold<TState> InitializedWith<TState>(TState startValue)
        {
            return new Fold<TState>(History.Concat(Changes), startValue);
        }

        public void Replay(IAmAnEventMessage eventMessage)
        {
            EventsToReplay.Add(eventMessage);
        }

        public string Id { get; protected internal set; }
    }
}