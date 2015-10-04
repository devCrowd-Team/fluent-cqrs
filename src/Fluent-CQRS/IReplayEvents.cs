namespace Fluent_CQRS
{
    public  interface IReplayEvents
    {
        void To(IHandleEvents eventHandler);
        void ToAllEventHandlers();
        IReplayEvents OfMessageType<T>() where T:IAmAnEventMessage;
    }
}