using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.DomainSample.Domain
{
    class ProductAggregate : Aggregate
    {
        public ProductAggregate(string id, IEnumerable<IAmAnEventMessage> history)
            : base(id, history)
        { }

        int InStock => MessagesOfType<ProductStocked>().Sum(p => p.Quantity)
                     - MessagesOfType<OrderlinePickingSucceeded>().Sum(p => p.Quantity);

        internal void StockProduct(int quantity)
        {
            if (MessagesOfType<ProductDefined>().Any())
                Changes.Add(new ProductStocked { Id = Id, Quantity = quantity });
        }

        internal void DefineProduct(string name)
        {
            if (!MessagesOfType<ProductDefined>().Any())
                Changes.Add(new ProductDefined { Id = Id, Name = name });
        }

        internal void PickOrderline(string orderId, int quantity)
        {
            if (InStock >= quantity)
                Changes.Add(new OrderlinePickingSucceeded { Id = Id, OrderId = orderId, Quantity = quantity });
            else
                Changes.Add(new OrderlinePickingFailed { Id = Id, OrderId = orderId, Quantity = quantity });
        }
    }
}
