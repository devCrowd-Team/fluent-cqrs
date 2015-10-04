using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Fluent_CQRS.Extensions;
using Fluent_CQRS.Tests.Infrastructure;
using NUnit.Framework;

namespace Fluent_CQRS.Tests
{
    [TestFixture]
    public class AggregateTests : With_Aggregates_Repository
    {
        [Test]
        public void When_execute_DoSomething_it_should_saved_one_SomethingHappend_event_in_the_eventstore()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.DoSomething());

            var aggregateEvents = _eventStore.RetrieveFor(testCommand.Id);

            aggregateEvents.OfType<SomethingHappend>().Count().Should().Be(1);
        }

        [Test]
        public void When_execute_DoSomethingOnce_twice_it_should_saved_one_SomethingHappend_event_in_the_eventstore()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.DoSomethingOnce());

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.DoSomethingOnce());

            var aggrigateEvents = _eventStore.RetrieveFor(testCommand.Id);

            aggrigateEvents.OfType<SomethingHappendOnce>().Count().Should().Be(1);
        }

        [Test]
        public void When_execute_DoSomethingOnce_twice_it_should_saved_one_SomethingHappend_event_in_the_eventstore_an_publish_twice()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.PublishNewStateTo(_eventHandler);

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.DoSomethingOnce());

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.DoSomethingOnce());

            var aggregateEvents = _eventStore.RetrieveFor(testCommand.Id);

            aggregateEvents.OfType<SomethingHappendOnce>().Count().Should().Be(1);

            _eventHandler.RecievedEvents.OfType<SomethingHappendOnce>().Count().Should().Be(2);
        }

        [Test]
        [ExpectedException(
            ExpectedException = typeof(ApplicationException),
            ExpectedMessage = "This is a intentionally Exception")]
        public void When_throw_an_Exception_within_the_aggregate_it_should_catched_in_CatchException_method()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowException())
                .CatchException(exception =>
                {
                    throw exception;
                });
        }

        [Test]
        [ExpectedException(
            ExpectedException = typeof(BusinessFault),
            ExpectedMessage = "My BusinessFault")]
        public void When_throw_a_Fault_within_the_aggregate_it_should_catched_in_CatchFault_method()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowFault())
                .CatchFault(fault =>
                {
                    throw fault;
                });
        }

        [Test]
        [ExpectedException(
            ExpectedException = typeof(ApplicationException),
            ExpectedMessage = "This is a intentionally Exception")]
        public void When_throw_an_Exception_within_the_aggregate_it_should_not_catched_by_default()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.ThrowException());
        }

        [Test]
        [ExpectedException(
            ExpectedException = typeof(BusinessFault),
            ExpectedMessage = "My BusinessFault")]
        public void When_throw_a_Fault_within_the_aggregate_it_should_not_catched_by_default()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Do(aggregate => aggregate.ThrowFault());
        }

        [Test]
        public void When_throw_an_Exception_within_the_aggregate_it_should_returned_by_execution_result()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            var executionResult = _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowException());

            executionResult.Exception.Should().BeOfType<ApplicationException>();
            executionResult.Exception.Message.Should().Be("This is a intentionally Exception");

        }

        [Test]
        public void When_throw_a_Fault_within_the_aggregate_it_should_returnes_by_execution_result()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            var executionResult = _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowFault());

            executionResult.Fault.Should().BeOfType<BusinessFault>();
            executionResult.Fault.Message.Should().Be("My BusinessFault");

        }

        [Test]
        public void When_throw_an_Exception_within_the_aggregate_it_should_not_saved_any_changes()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowException());

             _eventStore
                 .RetrieveFor(testCommand.Id)
                 .OfType<SomethingHappend>()
                 .Count().Should().Be(0);

        }

        [Test]
        public void When_throw_an_Fault_within_the_aggregate_it_should_not_saved_any_changes()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowException());

            _eventStore
                .RetrieveFor(testCommand.Id)
                .OfType<SomethingHappend>()
                .Count().Should().Be(0);

        }

        [Test]
        public void When_throw_an_Exception_within_the_aggregate_it_should_not_published_any_event()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };
            
            _aggregates.PublishNewStateTo(_eventHandler);

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowException());

            _eventHandler.RecievedEvents.Count.Should().Be(0);

        }

        [Test]
        public void When_throw_an_Fault_within_the_aggregate_it_should_not_published_any_event()
        {
            var testCommand = new TestCommand
            {
                Id = "TestAggr"
            };

            _aggregates.PublishNewStateTo(_eventHandler);

            _aggregates.Provide<TestAggregate>().With(testCommand)
                .Try(aggregate => aggregate.ThrowException());

            _eventHandler.RecievedEvents.Count.Should().Be(0);


        }
    }
}
