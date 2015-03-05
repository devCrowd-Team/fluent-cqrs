using System;

namespace Fluent_CQRS.Fluentation
{
    public class DoActionOnAggregate<TAggregate> where TAggregate : Aggregate
    {
        private readonly TAggregate _aggregate;
        private readonly AggregateStore _aggregateStore;

        public DoActionOnAggregate(TAggregate aggregate, AggregateStore aggregateStore)
        {
            _aggregate = aggregate;
            _aggregateStore = aggregateStore;
        }

        public AfterInvocation<TAggregate> Do(Action<TAggregate> doAction)
        {
            doAction.Invoke(_aggregate);

            return new AfterInvocation<TAggregate>(_aggregate, _aggregateStore);
        }
    }
}