using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Fold<TState> : IAggregateMessages<TState>
    {
        IEnumerable<IAmAnEventMessage> _messages;
        Mappings<TState> _mappings = new Mappings<TState>();
        Func<TState, TState> _otherwise = state => state;  //identity
        TState _startValue;

        public Fold(IEnumerable<IAmAnEventMessage> events)
            : this(events, default(TState))
        { }

        public Fold(IEnumerable<IAmAnEventMessage> messages, TState startValue)
        {
            _messages = messages;
            _startValue = startValue;
        }

        public Fold<TState> ApplyForAny<TMessage>(TState value)
            where TMessage : IAmAnEventMessage
        {
            return ApplyForAny<TMessage>((state, message) => value);
        }

        public Fold<TState> ApplyForAny<TMessage>(Func<TMessage, TState> apply)
            where TMessage : IAmAnEventMessage
        {
            return ApplyForAny<TMessage>((state, message) => apply(message));
        }

        public Fold<TState> ApplyForAny<TMessage>(Func<TState, TMessage, TState> apply)
            where TMessage : IAmAnEventMessage
        {
            _mappings.Add(apply);
            return this;
        }

        public IAggregateMessages<TState> Otherwise(TState v)
        {
            return Otherwise(state => v);
        }

        public IAggregateMessages<TState> Otherwise(Func<TState> f)
        {
            return Otherwise(state => f());
        }

        public IAggregateMessages<TState> Otherwise(Func<TState, TState> func)
        {
            _otherwise = func;
            return this;
        }

        public TState AggregateAllMessages()
        {
            var initialState = new ResultSet<TState>(_startValue, false);
            var dehydrated = _messages.Aggregate(initialState, (currentState, @event)
                => _mappings.Apply(currentState, @event));
            return dehydrated.Modified
                ? dehydrated.State
                : _otherwise(dehydrated.State);
        }
    }
}
