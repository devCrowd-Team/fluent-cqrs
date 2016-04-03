using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Events
{
    public class OrderlinePickingFailed : IAmAnEventMessage
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public int Quantity { get; set; }
    }
}