using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    public class Fold<TResult> : IAggregateMessages<TResult>
    {
        IEnumerable<IAmAnEventMessage> _events;
        IList<IFoldAction<TResult>> _foldActions;
        Func<TResult, TResult> _elseAction = state => state;  //identity
        TResult _startValue;

        internal Fold(IEnumerable<IAmAnEventMessage> events)
            : this(events, default(TResult))
        { }

        internal Fold(IEnumerable<IAmAnEventMessage> events, TResult startValue)
        {
            _events = events;
            _foldActions = new List<IFoldAction<TResult>>();
            _startValue = startValue;
        }

        public Fold<TResult> SetToAConstForAny<TEvent>(TResult value)
            where TEvent : IAmAnEventMessage
        {
            return ApplyForAny<TEvent>((@event, accumulator) => value);
        }

        public Fold<TResult> ApplyForAny<TEvent>(Func<TEvent, TResult> apply)
            where TEvent : IAmAnEventMessage
        {
            return ApplyForAny<TEvent>((@event, accumulator) => apply(@event));
        }

        public Fold<TResult> ApplyForAny<TEvent>(Func<TEvent, TResult, TResult> apply)
            where TEvent : IAmAnEventMessage
        {
            var action = new FoldAction<TEvent, TResult>
                ((@event, accumulator) => apply((TEvent)@event, accumulator))
                ;
            _foldActions.Add(action);
            return this;
        }

        public TResult AggregateAllMessages()
        {
            var actionApplied = false;
            var dehydrated =
                _events.Aggregate(_startValue, (result, @event) =>
                         _foldActions.Where(f => f.IsActionFor(@event))
                                     .Aggregate(result, (current, f) =>
                                     {
                                         actionApplied = true;
                                         return f.Apply(@event, current);
                                     }));
            return actionApplied ? dehydrated : _elseAction(dehydrated);
        }

        public IAggregateMessages<TResult> Else(TResult v)
        {
            return Else(state => v);
        }

        public IAggregateMessages<TResult> Else(Func<TResult, TResult> func)
        {
            _elseAction = func;
            return this;
        }
    }
}
