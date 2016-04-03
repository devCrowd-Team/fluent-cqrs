using Fluent_CQRS.DomainSample.Contracts.Events;
using System.Collections.Generic;
using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Values;

namespace Fluent_CQRS.DomainSample.Domain
{
    internal class OrderAggregate : Aggregate
    {
        public OrderAggregate(string id, IEnumerable<IAmAnEventMessage> history)
            : base(id, history) { }

        internal void Order(IEnumerable<Orderline> orderLines)
        {
            Changes.Add(new OrderPlaced
            {
                Id = Id,
                OrderLines = orderLines
            });
        }

      
    }
}