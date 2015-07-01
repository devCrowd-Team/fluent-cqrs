using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent_CQRS
{
    class EventHandlers : IConcatenateEventHandler
    {
        readonly IList<object> _receiver ;

        public EventHandlers()
        {
            _receiver = new List<object>();
        }
        public void Add(object eventHandler)
        {
            _receiver.Add(eventHandler);
        }

        public void Receive(IEnumerable<IAmAnEventMessage> events)
        {
            _receiver.ToList().ForEach(receiver => receiver
                .GetType()
                .InvokeMember("RecieveEvents", BindingFlags.InvokeMethod, null, receiver, new object[] {events}));
        }

        public IConcatenateEventHandler And(object eventHandler)
        {
            _receiver.Add(eventHandler);

            return this;
        }
    }
}