using System.Collections.Generic;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    internal class SampleAggregate : Aggregate
    {
        public SampleAggregate(string id, IEnumerable<IAmAnEventMessage> history)
            : base(id, history) { }

        public void SampleAggregateMethod(string myValue)
        {
            Changes.Add(new SampleEventRaised{MyValue = myValue});
        }
    }
}