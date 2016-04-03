using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Events
{
    public class ProductDefined:IAmAnEventMessage
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
