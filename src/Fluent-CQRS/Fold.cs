using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Fold<TResult> : IAggregateMessages<TResult>
    {
        IEnumerable<IAmAnEventMessage> _events;
        Aggregations<TResult> _aggregations = new Aggregations<TResult>();
        Func<TResult, TResult> _otherwise = state => state;  //identity
        TResult _startValue;

        internal Fold(IEnumerable<IAmAnEventMessage> events)
            : this(events, default(TResult))
        { }

        internal Fold(IEnumerable<IAmAnEventMessage> events, TResult startValue)
        {
            _events = events;
            _startValue = startValue;
        }

        public Fold<TResult> SetToAConstForAny<TEvent>(TResult value)
            where TEvent : IAmAnEventMessage
        {
            return ApplyForAny<TEvent>((state, message) => value);
        }

        public Fold<TResult> ApplyForAny<TEvent>(Func<TEvent, TResult> apply)
            where TEvent : IAmAnEventMessage
        {
            return ApplyForAny<TEvent>((state, message) => apply(message));
        }

        public Fold<TResult> ApplyForAny<TEvent>(Func<TResult, TEvent, TResult> apply)
            where TEvent : IAmAnEventMessage
        {
            _aggregations.Add(apply);
            return this;
        }

        public TResult AggregateAllMessages()
        {
            var initialState = new AggregationState<TResult>(false, _startValue);
            var dehydrated = _events.Aggregate(initialState, (currentState, @event) 
                => _aggregations.Apply(currentState, @event));
            return dehydrated.Applied
                ? dehydrated.Result
                : _otherwise(dehydrated.Result);
        }

        public IAggregateMessages<TResult> Otherwise(TResult v)
        {
            return Otherwise(state => v);
        }
        public IAggregateMessages<TResult> Otherwise(Func<TResult> f)
        {
            return Otherwise(state => f());
        }
        public IAggregateMessages<TResult> Otherwise(Func<TResult, TResult> func)
        {
            _otherwise = func;
            return this;
        }
    }
}
