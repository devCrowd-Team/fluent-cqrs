using System;

namespace Fluent_CQRS
{
    public interface ITellEventsToProcessManagers<TSaga>
    {
        ExecutionResult Handle(IAmAnEventMessage @event);
    }
}