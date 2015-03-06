using System;
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

        public void AndPublishTheNewState()
        {
            if(_aggregates.Publish.IsNotDefined())
                throw new ArgumentException("Keine Definition der Action Publish am Aggregates gefunden. Ein Publizieren der Events ist nicht möglich");

            _aggregates.Publish(_aggregate.Changes);
            _aggregate.Changes.Clear();
        }
    }
}