using System;
using Fluent_CQRS.Extensions;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    class Program
    {
        static void Main()
        {

            var aggregates = Aggregates.CreateWith(new InMemoryEventStore());

            var sampleEventHandler = new SampleEventHandler();
            var sampleCommandHandler = new SampleCommandHandler(aggregates);

            aggregates
                .SentEventsTo<SampleEventInformation>(sampleEventHandler.HandleMessage)
                .AndTo<ICarryEventInformation>(_ => { Console.WriteLine("--Message--\n"); });
            

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
