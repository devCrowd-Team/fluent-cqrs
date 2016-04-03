using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Fluent_CQRS
{
    public class Handler<TMessage>: IHandle<TMessage> ,IReceive<TMessage>
    {
        IList<Tuple<Type, Action<dynamic>>> messageActions = new List<Tuple<Type, Action<dynamic>>>();

        protected void Receive<T>(Action<T> handle) 
        {
            var messageType = typeof(T);
            messageActions.Add(new Tuple<Type, Action<dynamic>>(messageType, message => handle(message)));
        }
        
        public void Tell(TMessage message)
        {
            foreach (var t in messageActions)
            {
                var type = t.Item1;
                var handle = t.Item2;

                if (message.Is(type))
                {
                    handle(message);
                    break;
                }
            }
        }

        void IHandle<TMessage>.Receive(TMessage message)
        {
            Tell(message);
        }
    }
}
