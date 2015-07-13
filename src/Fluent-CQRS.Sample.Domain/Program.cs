using System;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    class Program
    {
        static void Main()
        {

            var aggregates = Aggregates.CreateWith(new InMemoryEventStore());

            var firstEventHandler = new SampleEventHandler();
            var secondEventHandler = new SampleEventHandler();
            var thirdEventHandler = new SampleEventHandler();
            var sampleCommandHandler = new SampleCommandHandler(aggregates);

            aggregates
                .PublishNewStateTo(firstEventHandler)
                .And(secondEventHandler)
                .And(thirdEventHandler);

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
