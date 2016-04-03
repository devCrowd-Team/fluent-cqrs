using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Events;
using Fluent_CQRS.DomainSample.Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent_CQRS.DomainSample.Domain
{
    class ProductReadModelEventHandler : Handler<IAmAnEventMessage>
    {
        private IList<ProductInStock> _productsReadModel;

        public ProductReadModelEventHandler(IList<ProductInStock> productsReadModel)
        {
            _productsReadModel = productsReadModel;

            Receive<ProductDefined>(@event => OnProductDefined(@event));
            Receive<ProductStocked>(@event => OnProductStacked(@event));
            Receive<OrderlinePickingSucceeded>(@event => OnProductPreparedForShipping(@event));
        }

        private void OnProductDefined(ProductDefined @event)
        {
            Console.WriteLine("ProductReadModelEventHandler: Event \"{0}\" with Id \"{1}\"", @event.GetType().Name, @event.Id);

            _productsReadModel.Add(new ProductInStock
            {
                Id = @event.Id,
                Name = @event.Name,
                Quantity = 0
            });
        }

        private void OnProductStacked(ProductStocked @event)
        {
            Console.WriteLine("ProductReadModelEventHandler: Event \"{0}\" with Id \"{1}\"", @event.GetType().Name, @event.Id);

            _productsReadModel
                .Single(p => p.Id == @event.Id)
                .Quantity += @event.Quantity;
        }

        private void OnProductPreparedForShipping(OrderlinePickingSucceeded @event)
        {
            Console.WriteLine("ProductReadModelEventHandler: Event \"{0}\" with Id \"{1}\"", @event.GetType().Name, @event.Id);

            _productsReadModel
                .Single(p => p.Id == @event.Id)
                .Quantity -= @event.Quantity;
        }
    }
}
