namespace Fluent_CQRS
{
    public abstract class RouteEventsTo<TProcessManager> : Handler<IAmAnEventMessage> where TProcessManager : ProcessManager
    {
        private ProcessContext _processContext;

        public RouteEventsTo(ProcessContext processContext)
        {
            _processContext = processContext;
        }

        public ITellEventsToProcessManagers<TProcessManager> With(CorrelationId correlationId)
        {
            return _processContext.Provide<TProcessManager>().With(correlationId);
        }
    }
}
