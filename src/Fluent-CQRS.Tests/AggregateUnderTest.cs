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
        public int NumberOfValueChanges
        {
            get{ return MessagesOfType<ValueChanged>().Count();}
        }

        public int SumOfValueChanges
        {
            get { return MessagesOfType<ValueChanged>().Sum(message => message.Value); }
        }

        public int SumOfAnyValues
        {
            get
            {
                return MessagesOfType<ValueInitialized>().Sum(message => message.Value)
                       + MessagesOfType<ValueChanged>().Sum(message => message.Value);
            }
        }

        public int SumOfAnyValuesAggregated
        {
            get
            {
                return InitializedAs<int>()
                    .ApplyForAny<ValueInitialized>((message) => message.Value)
                    .ApplyForAny<ValueChanged>((state, message) => state + message.Value)
                    .AggregateAllMessages();
            }
        }

        public int SumOfAllValuesAfterSomeThingHappened
        {
            get
            {
                return InitializedWith(0)
                    .ApplyForAny<ValueInitialized>((state, message) => state + message.Value)
                    .ApplyForAny<ValueChanged>((state, message) => state + message.Value)
                    .ApplyForAny<SomeThingElseHappened>(0)
                    .AggregateAllMessages();
            }
        }

        public string PassInitialState
        {
            get
            {
                return InitializedWith("CQRS")
                    .AggregateAllMessages();
            }
        }

        public string ReturnDefaultValue
        {
            get
            {
                return InitializedAs<string>()
                    .AggregateAllMessages();
            }
        }

        public string ElseCase
        {
            get
            {
                return InitializedAs<string>()
                    .ApplyForAny<NeverUsed>(message => "Hello World")
                    .Otherwise("Else")
                    .AggregateAllMessages();
            }
        }

        public string ElseCaseNotNeeded
        {
            get
            {
                return InitializedAs<string>()
                    .ApplyForAny<ValueInitialized>(message => "Else Not Needed")
                    .Otherwise("Else")
                    .AggregateAllMessages();
            }
        }
    }
}
