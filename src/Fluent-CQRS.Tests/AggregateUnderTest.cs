using Fluent_CQRS.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.Tests
{
    class AggregateUnderTest : Aggregate
    {
        public AggregateUnderTest(String id, IEnumerable<IAmAnEventMessage> history) : base(id, history)
        {
        }
        //you SHOULD NEVER set properties to public in production for aggregates
        public int NumberOfValueChanges => MessagesOfType<ValueChanged>().Count();

        public int SumOfValueChanges => MessagesOfType<ValueChanged>().Sum(message => message.Value);

        public int SumOfAnyValues => MessagesOfType<ValueInitialized>().Sum(message => message.Value)
                                    + MessagesOfType<ValueChanged>().Sum(message => message.Value);

        public int SumOfAnyValuesAggregated
            => InitializedAs<int>()
                   .ApplyForAny<ValueInitialized>((message) => message.Value)
                   .ApplyForAny<ValueChanged>(( state, message) => state + message.Value)
                   .AggregateAllMessages();

        public int SumOfAllValuesAfterSomeThingHappened
            => InitializedWith(0)
                   .ApplyForAny<ValueInitialized>(( state, message) => state + message.Value)
                   .ApplyForAny<ValueChanged>((state, message) => state + message.Value)
                   .SetToAConstForAny<SomeThingElseHappened>(0)
                   .AggregateAllMessages();

        public string PassInitialState
            => InitializedWith("CQRS")
                    .AggregateAllMessages();

        public string ReturnDefaultValue
            => InitializedAs<string>()
                    .AggregateAllMessages();

        public string ElseCase
            => InitializedAs<string>()
                    .ApplyForAny<NeverUsed>(message => "Hello World")
                    .Otherwise("Else")
                    .AggregateAllMessages();

        public string ElseCaseNotNeeded
            => InitializedAs<string>()
                    .ApplyForAny<ValueInitialized>(message => "Else Not Needed")
                    .Otherwise("Else")
                    .AggregateAllMessages();
    }
}
