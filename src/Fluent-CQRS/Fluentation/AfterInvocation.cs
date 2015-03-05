using Fluent_CQRS.Extensions;

namespace Fluent_CQRS.Fluentation
{
    public class AfterInvocation<TAggregate> where TAggregate : Aggregate
    {
        private readonly TAggregate _aggregate;
        private readonly AggregateStore _aggregateStore;

        public AfterInvocation(TAggregate aggregate, AggregateStore aggregateStore)
        {
            _aggregate = aggregate;
            _aggregateStore = aggregateStore;
        }

        public EventPublishing FinallySaveIt()
        {
            _aggregateStore.SaveChangesBy(_aggregate);

            if(_aggregateStore.Publish.IsNotDefined())
                _aggregate.Changes.Clear();

            return new EventPublishing(_aggregateStore, _aggregate);
        }
    }
}