using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Values;
using System.Collections.Generic;

namespace Fluent_CQRS.DomainSample.Contracts.Commands
{
    public class PlaceOrder:IAmACommandMessage
    {
        public string Id { get; set; }
        public IEnumerable<Orderline> OrderLines { get; set; }  
    }

   
}
