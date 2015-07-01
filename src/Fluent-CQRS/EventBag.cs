using System;

namespace Fluent_CQRS
{
    class EventBag
    {
        public String AggregateId { get; set; }
        public DateTime TimeStamp { get; set; }

        public Type TypeOfData { get; set; }
        public ICarryEventInformation Information { get; set; }
    }
}