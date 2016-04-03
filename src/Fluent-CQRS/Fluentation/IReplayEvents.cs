using System.Collections.Generic;

namespace Fluent_CQRS.Fluentation
{
    public  interface IReplayEvents
    {
        void To(IHandle<IEnumerable<IAmAnEventMessage>> eventHandler);
        void To(IHandleEvents eventHandler);
        void ToAllEventHandlers();
        IReplayEvents OfType<T>() where T:IAmAnEventMessage;
    }
}