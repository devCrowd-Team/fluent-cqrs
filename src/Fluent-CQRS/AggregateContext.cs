using System;
using System.Collections.Generic;
using Fluent_CQRS.Fluentation;

namespace Fluent_CQRS
{
    public  class AggregateContext
    {
        readonly IStoreAndRetrieveEvents _eventStore;
        readonly Handlers<IAmAnEventMessage> _eventHandlers;
 
        public static AggregateContext CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new AggregateContext(eventStore);
        }

        internal AggregateContext(IStoreAndRetrieveEvents eventStore)
        {
            _eventStore = eventStore;
            _eventHandlers = new Handlers<IAmAnEventMessage>();
        }

        public IProvideAnAggregate<TAggregate> Provide<TAggregate>() where TAggregate : Aggregate
        {
            var typeOfAggregate = typeof(TAggregate);

           return new AggregateLifeCycle<TAggregate>(_eventStore, TypedNewStateCallBack(typeOfAggregate));

        }

        public IConcatenateHandlers<IAmAnEventMessage> PublishNewStateTo(IHandle<IEnumerable<IAmAnEventMessage>> eventHandler)
        {
            _eventHandlers.Add(eventHandler);
            return _eventHandlers;
        }

        public IConcatenateHandlers<IAmAnEventMessage> PublishNewStateTo(IHandle<IAmAnEventMessage> eventHandler)
        {
            _eventHandlers.Add(eventHandler);
            return _eventHandlers;
        }



        public ICollectEvents ReplayFor<TAggregate>() where TAggregate : Aggregate
        {
            return new PlaybackEvents<TAggregate>(_eventStore, ReplayCallback);
        }

        void DispatchMessages(IEnumerable<IAmAnEventMessage> events)
        {
            _eventHandlers.Receive(events);
        }

        Action<IEnumerable<IAmAnEventMessage>> TypedNewStateCallBack(Type typeOfAggregate)
        {
            var focusedCallBack = new Action<IEnumerable<IAmAnEventMessage>>(events =>
            {
                  DispatchMessages(events);
            });
            return focusedCallBack;
        }

        void ReplayCallback(IEnumerable<IAmAnEventMessage> events)
        {
            DispatchMessages(events);
        }
    }
}