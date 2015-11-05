using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{

    class Aggregations<TResult>
    {
        IList<Func<AggregationState<TResult>, IAmAnEventMessage, AggregationState<TResult>>> _aggregations
            = new List<Func<AggregationState<TResult>, IAmAnEventMessage, AggregationState<TResult>>>();

        public AggregationState<TResult> Apply(AggregationState<TResult> startState, IAmAnEventMessage message)
        {
            return _aggregations.Aggregate(startState, (currentState, apply) => apply(currentState, message));
        }

        public void Add<TMessage>(Func<TResult, TMessage, TResult> apply)
        {
            _aggregations.Add((state, message) =>
            {
                if (message is TMessage)
                    return new AggregationState<TResult>(true, apply(state.Result, (TMessage)message));
                else
                    return state;
            });
        }
    }
}
