namespace Fluent_CQRS.Fluentation
{
    public interface IProvideAProcessManager<TProcessManager>
    {
        ITellEventsToProcessManagers<TProcessManager> With(CorrelationId id);
    }
}