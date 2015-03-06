using System;

namespace Fluent_CQRS.Fluentation
{
    public class DoActionOnAggregate<TAggregate> where TAggregate : Aggregate
    {
        private readonly TAggregate _aggregate;
        private readonly Aggregates _aggregates;

        public DoActionOnAggregate(TAggregate aggregate, Aggregates aggregates)
        {
            _aggregate = aggregate;
            _aggregates = aggregates;
        }

        public AfterInvocation<TAggregate> Do(Action<TAggregate> doAction)
        {
            doAction.Invoke(_aggregate);

            return new AfterInvocation<TAggregate>(_aggregate, _aggregates);
        }
    }
}