using System;

namespace Fluent_CQRS
{
    class EventBag
    {
        public String AggregateId { get; set; }
        public Type TypeOfEvent { get; set; }
        public IAmAnEventMessage Event { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}