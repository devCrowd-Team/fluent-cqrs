using Fluent_CQRS.Tests.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.Tests
{
    [TestFixture]
    public class StateDehydrationTests : With_AggregateUnderTest
    {
        [Test]
        public void When_messages_are_examined_with_MessagesOfType_they_can_be_counted()
        {
            var result = _aggregate.NumberOfValueChanges;
            Assert.AreEqual(4, result);
        }
        [Test]
        public void When_messages_are_examined_with_MessagesOfType_they_can_be_summed_up()
        {
            var result = _aggregate.SumOfValueChanges;
            Assert.AreEqual(14, result);
        }
        [Test]
        public void When_messages_are_examined_with_MessagesOfType_multiple_events_can_be_adressed()
        {
            var result = _aggregate.SumOfAnyValues;
            Assert.AreEqual(15, result);
        }
        [Test]
        public void When_messages_are_aggregated_they_can_be_summed_up()
        {
            var result = _aggregate.SumOfAnyValuesAggregated;
            Assert.AreEqual(15, result);
        }
        [Test]
        public void When_messages_are_aggregated_their_history_must_be_respected()
        {
            var result = _aggregate.SumOfAllValuesAfterSomeThingHappened;
            Assert.AreEqual(9, result);
        }
        [Test]
        public void When_messages_are_aggregated_the_state_can_be_initialized()
        {
            var result = _aggregate.PassInitialState;
            Assert.AreEqual("CQRS", result);
        }
        [Test]
        public void When_messages_are_aggregated_the_state_can_be_set_to_the_default_value_of_the_return_type()
        {
            var result = _aggregate.ReturnDefaultValue;
            Assert.AreEqual(null, result);
        }
        [Test]
        public void When_messages_are_aggregated_and_no_suitable_events_are_found_a_elsecase_can_be_defined()
        {
            var result = _aggregate.ElseCase;
            Assert.AreEqual("Else", result);
        }
        [Test]
        public void When_messages_are_aggregated_and__suitable_events_are_found_elsecase_is_ignored()
        {
            var result = _aggregate.ElseCaseNotNeeded;
            Assert.AreEqual("Else Not Needed", result);
        }
    }
}
