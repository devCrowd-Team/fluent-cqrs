using System;
using System.Collections.Generic;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    public sealed class Aggregates
    {
        readonly IStoreAndRetrieveEvents _eventStore;
        readonly EventHandlers _eventHandlers;
        readonly Dictionary<Type, FocusedEventHandlers> _focusedEventHandlers;

        public static Aggregates CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new Aggregates(eventStore);
        }

        internal Aggregates(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
            _eventHandlers = new EventHandlers();
            _focusedEventHandlers = new Dictionary<Type, FocusedEventHandlers>();
        }

        public IProvideAnAggregate<TAggregate> Provide<TAggregate>() where TAggregate : Aggregate
        {
            var typeOfAggregate = typeof(TAggregate);

            return _focusedEventHandlers.ContainsKey(typeOfAggregate)
                ? new AggregateLifeCycle<TAggregate>(_eventStore, TypedNewStateCallBack(typeOfAggregate)) 
                : new AggregateLifeCycle<TAggregate>(_eventStore, NewStateCallback);
        }

        public IConcatenateEventHandler PublishNewStateTo(IHandleEvents eventHandler)
        {
            _eventHandlers.Add(eventHandler);

            return _eventHandlers;
        }

        public IConcatenateEventHandlersForAnAggregate PublishNewStateOf<TAggregate>()
        {
            var typeOfAggregate = typeof(TAggregate);

            if (!_focusedEventHandlers.ContainsKey(typeOfAggregate))
            {
                _focusedEventHandlers.Add(typeOfAggregate, new FocusedEventHandlers());

            }

            return _focusedEventHandlers[typeOfAggregate]; ;

        }

        public ICollectEvents ReplayFor<TAggregate>() where TAggregate : Aggregate
        {
            return new PlaybackEvents<TAggregate>(_eventStore, ReplayCallback);
        }

        void NewStateCallback(IEnumerable<IAmAnEventMessage> events)
        {
            _eventHandlers.Receive(events);
        }

        Action<IEnumerable<IAmAnEventMessage>> TypedNewStateCallBack(Type typeOfAggregate)
        {
            var focusedCallBack = new Action<IEnumerable<IAmAnEventMessage>>(events =>
            {
                _focusedEventHandlers[typeOfAggregate].NewStateCallback(events);
                NewStateCallback(events);
            });
            return focusedCallBack;
        }

        void ReplayCallback(IEnumerable<IAmAnEventMessage> events)
        {
            _eventHandlers.Receive(events);
        }
    }
}