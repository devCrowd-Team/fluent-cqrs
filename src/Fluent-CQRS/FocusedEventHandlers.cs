using System.Collections.Generic;

namespace Fluent_CQRS
{
    class FocusedEventHandlers : IConcatenateEventHandlersForAnAggregate
    {
        readonly EventHandlers _eventHandlers;

        public FocusedEventHandlers()
        {
            _eventHandlers = new EventHandlers();
        }

        public IConcatenateEventHandler To(IHandleEvents eventHandler)
        {
            _eventHandlers.Add(eventHandler);


            return _eventHandlers;
        }

        public void NewStateCallback(IEnumerable<IAmAnEventMessage> events)
        {
            _eventHandlers.Receive(events);
        }
    }
}