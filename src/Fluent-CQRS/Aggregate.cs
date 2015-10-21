using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Aggregate
    {
        public Aggregate(String id, IEnumerable<IAmAnEventMessage> history)
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

        public void Replay(IAmAnEventMessage eventMessage)
        {
            EventsToReplay.Add(eventMessage);
        }

        public String Id { get; protected internal set; }
    }
}