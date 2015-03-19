using System;

namespace Fluent_CQRS.Fluentation
{
    public interface IInvokeActionsOnAggregates<TAggregate>
    {
        ISaveAggregates Do(Action<TAggregate> doAction);
    }
}