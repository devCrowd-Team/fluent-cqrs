using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS.Tests.Infrastructure
{
    class EmptyTestAggregate : Aggregate
    {
        public EmptyTestAggregate(string id, IEnumerable<IAmAnEventMessage> history) : base(id, history)
        {
            if (MessagesOfType<InstanceCreated>().Any()) return;

            Changes.Add(new InstanceCreated
            {
                Id = Id
            });
        }

        public void DoNothing() { }
    }
}