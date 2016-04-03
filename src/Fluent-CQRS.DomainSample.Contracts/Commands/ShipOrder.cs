using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Commands
{
    public class ShipOrder : IAmACommandMessage
    {
        public string Id { get; set; }
    }
}