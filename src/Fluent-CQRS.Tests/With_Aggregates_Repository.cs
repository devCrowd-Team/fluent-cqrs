using Fluent_CQRS.Tests.Infrastructure;
using NUnit.Framework;

namespace Fluent_CQRS.Tests
{
    public class With_Aggregates_Repository
    {
        internal InMemoryEventStore _eventStore;
        internal Aggregates _aggregates;
        internal TestEventHandler _eventHandler;

        [SetUp]
        public void Setup()
        {
            _eventStore = new InMemoryEventStore();
            _aggregates = Aggregates.CreateWith(_eventStore);

            _eventHandler = new TestEventHandler();
        }
    }
}