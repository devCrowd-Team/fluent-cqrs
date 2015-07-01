using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    internal class SampleAggregate : Aggregate
    {
        public SampleAggregate(string id)
            : base(id) { }

        public void SampleAggregateMethod(string myValue)
        {
            Changes.Add(new SampleEventInformation{MyValue = myValue});
        }
    }
}