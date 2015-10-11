using System.Collections;
using System.Collections.Generic;

namespace Fluent_CQRS.Tests.Infrastructure
{
    class AlternativeTestAggregate : Aggregate
    {
        public AlternativeTestAggregate(string id, IEnumerable<IAmAnEventMessage> history) : base(id, history)
        {
        }

        public void DoAlsoSomething()
        {
            Changes.Add(new SomethingHappend());
        }
    }
}