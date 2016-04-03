using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Events
{
    public class ProductStocked  : IAmAnEventMessage
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
