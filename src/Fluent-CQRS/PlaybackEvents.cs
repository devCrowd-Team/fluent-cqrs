using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS
{
    internal class PlaybackEvents<TAggregate> : ICollectEvents, IReplayEvents where TAggregate : Aggregate
    {
        readonly IStoreAndRetrieveEvents _eventStore;
        readonly Action<IEnumerable<IAmAnEventMessage>> _replayCallback;
        IEnumerable<IAmAnEventMessage> _currentAggregateEvents;

        public PlaybackEvents(IStoreAndRetrieveEvents eventStore, Action<IEnumerable<IAmAnEventMessage>> replayCallback)
        {
            _eventStore = eventStore;
            _replayCallback = replayCallback;
        }

        public IReplayEvents EventsWithAggregateId(string aggregateId)
        {
            _currentAggregateEvents = _eventStore.RetrieveFor<TAggregate>(aggregateId);

            return this;
        }

        public IReplayEvents AllEvents()
        {
            _currentAggregateEvents = _eventStore.RetrieveFor<TAggregate>();

            return this;
        }

        public void To(IHandleEvents eventHandler)
        {
            eventHandler.Receive(_currentAggregateEvents);
        }

        public void ToAllEventHandlers()
        {
            _replayCallback(_currentAggregateEvents);
        }

        public IReplayEvents OfType<TEvent>() where TEvent : IAmAnEventMessage
        {
            _currentAggregateEvents = _currentAggregateEvents.OfType<TEvent>() as IEnumerable<IAmAnEventMessage> ;
            return this;
        }
    }
}