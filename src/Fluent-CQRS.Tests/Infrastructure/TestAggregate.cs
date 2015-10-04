using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.Tests.Infrastructure
{
    class TestAggregate : Aggregate
    {
        public TestAggregate(string id)
            : base(id)
        {
        }

        public void DoSomething()
        {
            Changes.Add(new SomethingHappend());
        }

        public void DoSomethingOnce()
        {
            if (MessagesOfType<SomethingHappendOnce>().Any())
            {
                Replay(new SomethingHappendOnce());
            }
            else
            {
                Changes.Add(new SomethingHappendOnce());
            }
        }

        public void ThrowException()
        {
            Changes.Add(new SomethingHappend());

            throw new ApplicationException("This is a intentionally Exception");
        }

        public void ThrowFault()
        {
            Changes.Add(new SomethingHappend());

            throw new BusinessFault();
        }

        public void DoFourActions()
        {
            Changes.Add(new SomtethingSpecialHappend
            {
                NiceProperty = "Test1"
            });

            Changes.Add(new SomtethingSpecialHappend
            {
                NiceProperty = "Test2"
            });

            Changes.Add(new SomtethingSpecialHappend
            {
                NiceProperty = "Test3"
            });

            Changes.Add(new SomtethingSpecialHappend
            {
                NiceProperty = "Test4"
            });
        }
    }
}
