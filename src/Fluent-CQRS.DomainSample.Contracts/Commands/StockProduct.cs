using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Commands
{
    public class StockProduct  : IAmACommandMessage
    {
        public string Id { get; set; }
        public int  Quantity { get; set; }
    }
}
