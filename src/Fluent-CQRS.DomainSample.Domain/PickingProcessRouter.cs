using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Events;
using System;

namespace Fluent_CQRS.DomainSample.Domain
{
    class PickingProcessRouter : RouteEventsTo<PickingProcessManager>
    {
        public PickingProcessRouter(ProcessContext processContext):base(processContext )
        {
            Receive<ProductStocked>(@event => With(new CorrelationId ( @event.Id)).Handle(@event));
            Receive<OrderlinePickingSucceeded>(@event => With(new CorrelationId(@event.Id)).Handle(@event));
            Receive<OrderlinePickingFailed>(@event => With(new CorrelationId(@event.Id)).Handle(@event));
        }
    }
}
