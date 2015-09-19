using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.Tests.Infrastructure
{
    class TestAggregate : Aggregate
    {
        public TestAggregate(string id) : base(id)
        {
        }

        public void DoSomething()
        {
            Changes.Add(new SomethingHappend());
        }

        public void DoSomethingOnce()
        {
            if (MessagesOfType<SomethingHappendOnce>().Any()) return;

            Changes.Add(new SomethingHappendOnce());
        }
    }
}
