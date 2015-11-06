using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    class Mappings<TState>
    {
        IList<Func<ResultSet<TState>, IAmAnEventMessage, ResultSet<TState>>> _mappings
            = new List<Func<ResultSet<TState>, IAmAnEventMessage, ResultSet<TState>>>();

        public ResultSet<TState> Apply(ResultSet<TState> startState, IAmAnEventMessage message)
        {
            return _mappings.Aggregate(startState, (currentState, apply) => apply(currentState, message));
        }

        public void Add<TMessage>(Func<TState, TMessage, TState> apply) where TMessage : IAmAnEventMessage
        {
            _mappings.Add((state, message) =>
            {
                if (message is TMessage)
                    return new ResultSet<TState>(apply(state.State, (TMessage)message), true);
                else
                    return state;
            });
        }
    }
}
