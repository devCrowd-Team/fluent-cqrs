using System;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    public class SampleCommandHandler
    {
        readonly AggregateStore _aggregateStore;

        public SampleCommandHandler(AggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }

        public void Handle(SampleDomainCommand command)
        {
            Console.WriteLine("Command recieved");

            _aggregateStore.Provide<SampleAggregate>().With(command)
                .Do(aggregate=>aggregate.SampleAggregateMethod(command.MyValue))
                .FinallySaveIt()
                .AndPublishTheNewState();
        }
    }
}