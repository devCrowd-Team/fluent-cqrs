using Fluent_CQRS.Tests.Infrastructure;
using NUnit.Framework;
using System.Linq;
using FluentAssertions;

namespace Fluent_CQRS.Tests
{
    [TestFixture]
    public class AggregatesRepositoryTests : With_Aggregates_Repository
    {
        [Test]
        public void When_an_aggregate_fires_4_events_after_creating_then_should_the_repository_replay_only_this_4_events()
        {
            var aggrId = "TestAggr";

            _aggregates
                .PublishNewStateTo(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoFourActions());

            _eventStore.RetrieveFor(aggrId).Count().Should().Be(4);

            _eventHandler.RecievedEvents.Clear();

            _aggregates
                .ReplayFor<TestAggregate>()
                .EventsWithAggregateId(aggrId)
                .ToAllEventHandlers();

            _eventHandler.RecievedEvents.Count.Should().Be(4);
        }

        [Test]
        public void When_replay_from_an_Aggregate_into_alternative_Event_Handler_it_should_have_the_same_events_like_the_default_Event_Handler()
        {
            var aggrId = "TestAggr";

            _aggregates
                .PublishNewStateTo(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoFourActions());

            var alternativeEventHandler = new AlternativeTestEventHandler();

            _aggregates
                .ReplayFor<TestAggregate>()
                .EventsWithAggregateId(aggrId)
                .To(alternativeEventHandler);

            _eventHandler.RecievedEvents.Count.Should().Be(4);
            alternativeEventHandler.RecievedEvents.Count.Should().Be(4);

            _eventHandler.RecievedEvents.ShouldAllBeEquivalentTo(alternativeEventHandler.RecievedEvents);
        }

        [Test]
        public void When_replay_Events_it_should_only_replay_from_a_specific_Aggregate()
        {
            var aggrId = "TestAggr";

            _aggregates
                .PublishNewStateTo(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoFourActions());

            _aggregates
                .Provide<AlternativeTestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoAlsoSomething());

            _eventStore.RetrieveFor(aggrId).Count().Should().Be(5);

            _eventHandler.RecievedEvents.Clear();

            _aggregates
                .ReplayFor<TestAggregate>()
                .EventsWithAggregateId(aggrId)
                .ToAllEventHandlers();

            _eventHandler.RecievedEvents.Count.Should().Be(4);
        }

        [Test]
        public void When_replay_Events_of_only_one_type_it_should_ignore_all_other()
        {
            var aggrId = "TestAggr";

            _aggregates
                .PublishNewStateTo(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoSomethingOnce());

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoSomething());

            _eventStore.RetrieveFor(aggrId).Count().Should().Be(2);

            _eventHandler.RecievedEvents.Clear();

            _aggregates
                .ReplayFor<TestAggregate>()
                .EventsWithAggregateId(aggrId)
                .OfType<SomethingHappend>()
                .ToAllEventHandlers();

            _eventHandler.RecievedEvents.Count.Should().Be(1);
        }

        [Test]
        public void When_replay_Events_it_should_replay_all_events_for_aggregate_type()
        {
            var aggrId = "TestAggr";
            var otherAggrId = "OtherTestAggr";

            _aggregates
                .PublishNewStateTo(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoSomething());

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = otherAggrId })
                .Do(aggr => aggr.DoSomething());

            _eventStore.RetrieveFor(aggrId).Count().Should().Be(1);
            _eventStore.RetrieveFor(otherAggrId).Count().Should().Be(1);

            _eventHandler.RecievedEvents.Clear();

            _aggregates
                .ReplayFor<TestAggregate>()
                .AllEvents()
                .ToAllEventHandlers();

            _eventHandler.RecievedEvents.Count.Should().Be(2);
        }

        [Test]
        public void When_replay_Events_it_should_replay_only_events_for_that_aggregate_type()
        {
            var aggrId = "TestAggr";

            _aggregates
                .PublishNewStateTo(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoSomething());

            _aggregates
                .Provide<AlternativeTestAggregate>()
                .With(new TestCommand { Id = aggrId })
                .Do(aggr => aggr.DoAlsoSomething());

            _eventStore.RetrieveFor(aggrId).Count().Should().Be(2);

            _eventHandler.RecievedEvents.Clear();

            _aggregates
                .ReplayFor<TestAggregate>()
                .AllEvents()
                .ToAllEventHandlers();

            _eventHandler.RecievedEvents.Count.Should().Be(1);
        }

        [Test]
        public void When_get_an_Aggregate_by_AggregateId()
        {
            var aggrId = "Test";

            _aggregates
                .Provide<TestAggregate>()
                .With(new AggregateId(aggrId))
                .Do(aggr => aggr.Id.Should().Be(aggrId));
        }

        [Test]
        public void When_publish_events_from_TestAggregate_it_should_handle_the_events_at_TestEventHandler()
        {
            _aggregates.PublishNewStateOf<TestAggregate>().To(_eventHandler);

            _aggregates
                .Provide<TestAggregate>()
                .With(new AggregateId("Test"))
                .Do(aggregate => aggregate.DoSomethingOnce());

            _eventHandler.RecievedEvents.Count.Should().Be(1);
        }
    }
}
