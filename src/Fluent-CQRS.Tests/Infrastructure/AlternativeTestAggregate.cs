namespace Fluent_CQRS.Tests.Infrastructure
{
    class AlternativeTestAggregate : Aggregate
    {
        public AlternativeTestAggregate(string id) : base(id)
        {
        }

        public void DoAlsoSomething()
        {
            Changes.Add(new SomethingHappend());
        }
    }
}