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
        public int SumOfValueChanges => MessagesOfType<ValueChanged>().Sum(e => e.Value);
        public int SumOfAnyValues => MessagesOfType<ValueInitialized>().Sum(e => e.Value)
                                    + MessagesOfType<ValueChanged>().Sum(e => e.Value);
        public int SumOfAnyValuesAggregated
            => WithAnIntitialStateAs<int>()
                   .ApplyForAny<ValueInitialized>((message) => message.Value)
                   .ApplyForAny<ValueChanged>((message, state) => state + message.Value)
                   .AggregateAllMessages();

        public int SumOfAllValuesAfterSomeThingHappened
            => WithAnInitialStateOf(0)
                   .ApplyForAny<ValueInitialized>((@event, state) => state + @event.Value)
                   .ApplyForAny<ValueChanged>((@event, state) => state + @event.Value)
                   .SetToAConstForAny<SomeThingElseHappened>(0)
                   .AggregateAllMessages();
        public string PassInitialState
            => WithAnInitialStateOf("CQRS")
                    .AggregateAllMessages();
        public string ReturnDefaultValue
            => WithAnIntitialStateAs<string>()
                    .AggregateAllMessages();
        public string ElseCase
            => WithAnIntitialStateAs<string>()
                    .ApplyForAny<NeverUsed>(e => "Hello World")
                    .Else("Else")
                    .AggregateAllMessages();
        public string ElseCaseNotNeeded
            => WithAnIntitialStateAs<string>()
                    .ApplyForAny<ValueInitialized>(e => "Else Not Needed")
                    .Else("Else")
                    .AggregateAllMessages();
    }
}
