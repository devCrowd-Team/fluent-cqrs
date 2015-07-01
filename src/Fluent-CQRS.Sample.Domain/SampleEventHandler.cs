using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    public class SampleEventHandler
    {
        public void RecieveEvents(IEnumerable<IAmAnEventMessage> eventMessages)
        {
            eventMessages.ToList().ForEach(message => this.GetType().InvokeMember("HandleMessage", BindingFlags.InvokeMethod, null, this, new object[]{message}));
        }

        public void HandleMessage(SampleEventRaised message)
        {
            Console.WriteLine("Event {0} empfangen: {1}", message.GetType(),message.MyValue);
        }

        
    }
}
