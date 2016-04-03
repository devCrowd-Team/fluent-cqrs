using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Events;

namespace Fluent_CQRS.DomainSample.Domain
{
    class OrderProcessRouter : RouteEventsTo<OrderProcessManager>
    {
        public OrderProcessRouter(ProcessContext processContext) : base(processContext)
        {
            Receive<OrderPlaced>(@event => With(new CorrelationId(@event.Id)).Handle(@event));
            Receive<OrderlinePickingSucceeded>(@event => With(new CorrelationId(@event.OrderId)).Handle(@event));
            Receive<OrderlinePickingFailed>(@event => With(new CorrelationId(@event.OrderId)).Handle(@event));
        }
    }
}
