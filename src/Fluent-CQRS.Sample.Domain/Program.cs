using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    class Program
    {
        static void Main(string[] args)
        {

            var aggregateStore = new Aggregates(new InMemoryEventStore());

            var sampleEventHandler = new SampleEventHandler();
            var sampleCommandHandler = new SampleCommandHandler(aggregateStore);

            aggregateStore.Publish = sampleEventHandler.RecieveEvents;

            var aggregateId = Guid.NewGuid().ToString();

            sampleCommandHandler.Handle(
                new SampleDomainCommand
                {
                    Id = aggregateId,
                    MyValue = "Hallo Welt 1"
                });

            sampleCommandHandler.Handle(
                new SampleDomainCommand
                {
                    Id = aggregateId,
                    MyValue = "Hallo Welt 2"
                });

            sampleCommandHandler.Handle(
                new SampleDomainCommand
                {
                    Id = aggregateId,
                    MyValue = "Hallo Welt 3"
                });

            Console.ReadLine();

        }
    }
}
