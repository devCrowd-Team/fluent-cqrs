using System;
using System.IO;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    public class SampleCommandHandler
    {
        readonly Aggregates _aggregates;

        public SampleCommandHandler(Aggregates aggregates)
        {
            _aggregates = aggregates;
        }

        public void Handle(SampleDomainCommand command)
        {
            Console.WriteLine("Command {0} recieved: {1}", command.Id, command.MyValue);

            _aggregates.Provide<SampleAggregate>().With(command)
                .Do(aggregate => aggregate.SampleAggregateMethod(command.MyValue))
                .FinallySaveIt()
                .AndPublishNewState();
        }
    }
}