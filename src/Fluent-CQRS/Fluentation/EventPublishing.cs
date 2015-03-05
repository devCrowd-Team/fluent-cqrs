using System;
using Fluent_CQRS.Extensions;

namespace Fluent_CQRS.Fluentation
{
    public class EventPublishing
    {
        private readonly AggregateStore _aggregateStore;
        private readonly Aggregate _aggregate;

        public EventPublishing(AggregateStore aggregateStore, Aggregate aggregate)
        {
            _aggregateStore = aggregateStore;
            _aggregate = aggregate;
        }

        public void AndPublishTheNewState()
        {
            if(_aggregateStore.Publish.IsNotDefined())
                throw new ArgumentException("Keine Definition der Action Publish am AggregateStore gefunden. Ein Publizieren der Events ist nicht möglich");

            _aggregateStore.Publish(_aggregate.Changes);
            _aggregate.Changes.Clear();
        }
    }
}