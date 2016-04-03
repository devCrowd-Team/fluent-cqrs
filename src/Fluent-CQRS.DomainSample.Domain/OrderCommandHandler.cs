using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Commands;
using System;

namespace Fluent_CQRS.DomainSample.Domain
{
    class OrderCommandHandler : CommandHandlerFor<OrderAggregate>
    {
        public OrderCommandHandler(AggregateContext aggregateContext) : base(aggregateContext)
        {
            Receive<PlaceOrder>(cmd => With(cmd).Do(a => a.Order(cmd.OrderLines)));
        }
    }
}
