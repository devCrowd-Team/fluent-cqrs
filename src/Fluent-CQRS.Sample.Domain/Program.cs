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

            var aggregates = new Aggregates(new InMemoryEventStore());

            var sampleEventHandler = new SampleEventHandler();
            var sampleCommandHandler = new SampleCommandHandler(aggregates);

            aggregates.PublishNewState = sampleEventHandler.RecieveEvents;

            var aggregateId = Guid.NewGuid().ToString();

            sampleCommandHandler.Handle(
                new SampleDomainCommand
                {
                    Id = aggregateId,
                    MyValue = "Hi There"
                });

            sampleCommandHandler.Handle(
                new SampleDomainCommand
                {
                    Id = aggregateId,
                    MyValue = "Hello Kitty"
                });

            sampleCommandHandler.Handle(
                new SampleDomainCommand
                {
                    Id = aggregateId,
                    MyValue = "Hey Dude"
                });

            Console.ReadLine();

        }
    }
}
