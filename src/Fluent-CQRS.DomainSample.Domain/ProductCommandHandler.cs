
using Fluent_CQRS;
using Fluent_CQRS.DomainSample.Contracts.Commands;

namespace Fluent_CQRS.DomainSample.Domain
{
    class ProductCommandHandler : CommandHandlerFor<ProductAggregate>
    {
        public ProductCommandHandler(AggregateContext aggregateContext) : base(aggregateContext)
        {
            Receive<DefineProduct>(cmd => With(cmd).Do(a => a.DefineProduct(cmd.Name)));
            Receive<StockProduct>(cmd => With(cmd).Do(a => a.StockProduct(cmd.Quantity)));
            Receive<PickOrderline>(cmd => With(cmd).Do(a => a.PickOrderline(cmd.OrderId, cmd.Quantity)));
        }
    }
}
