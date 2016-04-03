using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Commands
{
    public class PickOrderline:IAmACommandMessage
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public int Quantity { get; set; }
    }
}
