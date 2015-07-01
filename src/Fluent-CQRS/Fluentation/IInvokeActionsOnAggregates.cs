using System;

namespace Fluent_CQRS.Fluentation
{
    public interface IInvokeActionsOnAggregates<out TAggregate>
    {
        ExecutionResult<ExecutionFlags> Do(Action<TAggregate> doAction);
    }
}