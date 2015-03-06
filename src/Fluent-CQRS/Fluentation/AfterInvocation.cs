using Fluent_CQRS.Extensions;

namespace Fluent_CQRS.Fluentation
{
    public class AfterInvocation<TAggregate> where TAggregate : Aggregate
    {
        private readonly TAggregate _aggregate;
        private readonly Aggregates _aggregates;

        public AfterInvocation(TAggregate aggregate, Aggregates aggregates)
        {
            _aggregate = aggregate;
            _aggregates = aggregates;
        }

        public EventPublishing FinallySaveIt()
        {
            _aggregates.SaveChangesBy(_aggregate);

            if(_aggregates.Publish.IsNotDefined())
                _aggregate.Changes.Clear();

            return new EventPublishing(_aggregates, _aggregate);
        }
    }
}