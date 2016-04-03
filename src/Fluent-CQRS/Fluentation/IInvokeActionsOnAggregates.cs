using System;

namespace Fluent_CQRS
{
    public interface IInvokeActionsOnAggregates<TAggregate>
    {
        void Do(Action<TAggregate> doAction);
        ExecutionResult Try(Action<TAggregate> doAction);
    }
}