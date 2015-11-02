using System;

namespace Fluent_CQRS
{
    internal class FoldAction<TEvent, TResult> : IFoldAction<TResult>
        where TEvent : IAmAnEventMessage
    {
        public Func<IAmAnEventMessage, bool> IsActionFor => e => e is TEvent;

        public Func<TEvent, TResult, TResult> Apply { get; private set; }
        Func<IAmAnEventMessage, TResult, TResult> IFoldAction<TResult>.Apply
            => (e, a) => Apply((TEvent)e, a);

        public FoldAction(Func<TEvent, TResult, TResult> apply)
        {
            Apply = apply;
        }
    }
}
