using Fluent_CQRS.Extensions;
using Fluent_CQRS.Fluentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS
{
    internal class Handlers<TMessage> : IConcatenateHandlers<TMessage>, IHandle<IEnumerable<TMessage>>
    {
        readonly IList<IHandle<IEnumerable<TMessage>>> _receivers = new List<IHandle<IEnumerable<TMessage>>>();

        public IConcatenateHandlers<TMessage> And(IHandle<IEnumerable<TMessage>> eventHandler)
        {
           Add(eventHandler);
            return this;
        }

        public IConcatenateHandlers<TMessage> And(IHandle<TMessage> eventHandler)
        {
            Add(eventHandler);
            return this;
        }


        public void Receive(IEnumerable<TMessage> messages)
        {
            foreach (var receiver in _receivers)
            {
                receiver.Receive(messages);
            }
        }

        internal void Add(IHandle<IEnumerable<TMessage>> eventHandler)
        {
            _receivers.Add(eventHandler);
        }

        internal void Add(IHandle<TMessage> eventHandler)
        {
            Add(eventHandler.ForMessages());
        }
       
    }
}