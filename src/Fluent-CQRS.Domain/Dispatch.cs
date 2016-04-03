using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Dispatch<T> : IHandle<T>, IReceive<T> where T : class
    {
        IList<Handler<T>> handlers = new List<Handler<T>>();

        internal Dispatch() { }

        public static Dispatch<T> To(Handler<T> handler)
        {
            var d = new Dispatch<T>();
            return d.And(handler);
        }
        public Dispatch<T> And(Handler<T> handler)
        {
            handlers.Add(handler);
            return this;
        }

        public void Tell(T message)
        {
            foreach (var h in handlers)
            {
                h.Tell(message);
            }   
        }

        void IHandle<T>.Receive(T message)
        {
            Tell(message);
        }
    }
}