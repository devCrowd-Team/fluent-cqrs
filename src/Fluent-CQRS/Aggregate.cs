using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Aggregate
    {
        public Aggregate(String id)
        {
            Changes = new List<IAmAnEventMessage>();
            History = new List<IAmAnEventMessage>();
            EventsToReplay = new List<IAmAnEventMessage>();
            
            Id = id;
        }

        public IEnumerable<IAmAnEventMessage> History { get; set; }

        public IList<IAmAnEventMessage> Changes { get; private set; }

        internal IList<IAmAnEventMessage> EventsToReplay { get; private set; }

        public IEnumerable<T> MessagesOfType<T>() where T : IAmAnEventMessage
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