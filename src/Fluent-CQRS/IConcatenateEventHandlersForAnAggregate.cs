namespace Fluent_CQRS
{
    public interface IConcatenateEventHandlersForAnAggregate
    {
        IConcatenateEventHandler To(IHandleEvents eventHandler);
    }
}