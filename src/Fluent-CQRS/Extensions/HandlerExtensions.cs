using System.Collections.Generic;

namespace Fluent_CQRS.Extensions
{
    internal static class HandlerExtensions
    {
        private class Converter<TMessage> : IHandle<IEnumerable<TMessage>>
        {
            private IHandle<TMessage> _receiver;

            internal Converter(IHandle<TMessage> receiver)
            {
                _receiver = receiver;
            }

            public void Receive(IEnumerable<TMessage> messages)
            {
                foreach (var message in messages)
                {
                    _receiver.Receive(message);
                }
            }
        }

        static internal IHandle<IEnumerable<TMessage>> ForMessages<TMessage>(this IHandle<TMessage> receiver)
        {
            return new Converter<TMessage>(receiver);
        }

       
    }
}

