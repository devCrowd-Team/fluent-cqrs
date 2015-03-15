using Fluent_CQRS.Extensions;

namespace Fluent_CQRS.Fluentation
{
    public class EventPublishing
    {
        private readonly Aggregates _aggregates;
        private readonly Aggregate _aggregate;

        public EventPublishing(Aggregates aggregates, Aggregate aggregate)
        {
            _aggregates = aggregates;
            _aggregate = aggregate;
        }

        public void AndPublishNewState()
        {
            if (_aggregates.PublishNewState.IsNotDefined())
                throw new MissingEventsPublishingTarget();

            _aggregates.PublishNewState(_aggregate.Changes);
            _aggregate.Changes.Clear();
        }
    }
}