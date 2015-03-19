using System;
using System.Collections.Generic;
using System.Linq;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    public class SampleEventHandler
    {
        public void RecieveEvents(IEnumerable<IAmAnEventMessage> eventMessages)
        {
            eventMessages.ToList().ForEach(message => HandleMessage((dynamic)message));
        }

        void HandleMessage(SampleEventRaised message)
        {
            Console.WriteLine("Event {0} empfangen: {1}", message.GetType(),message.MyValue);
        }

        void Handle(object unknownMessage)
        {
            throw new ArgumentException(
                String.Format("Dieser Event Typ {0} ist keinem Handler zugeordnet",
                unknownMessage.GetType()));
        }
    }
}
