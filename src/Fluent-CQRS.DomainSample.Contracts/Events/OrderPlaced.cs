using System.Collections.Generic;
using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Values;

namespace Fluent_CQRS.DomainSample.Contracts.Events
{
    public class OrderPlaced : IAmAnEventMessage
    {
        public string Id { get; set; }
        public IEnumerable<Orderline> OrderLines { get; set; }
    }
}