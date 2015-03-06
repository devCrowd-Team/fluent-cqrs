using System;

namespace Fluent_CQRS
{
    public class MissingEventsPublishingTarget : Exception
    {
        public MissingEventsPublishingTarget() : base(
            "There is no target to recieve the Events. " +
            "Assign a Action<IEnumerable<IAmAnEventMessage>> to Aggregates.PublishNewState")
        {
        }
    }
}