using System;

namespace Fluent_CQRS.Fluentation
{
    public interface IInvokeActionsOnAggregates<TAggregate>
    {
        ExecutionResult Do(Action<TAggregate> doAction);
        ExecutionResult Try(Action<TAggregate> doAction);
    }
}