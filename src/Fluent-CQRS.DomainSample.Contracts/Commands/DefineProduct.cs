using Fluent_CQRS;

namespace Fluent_CQRS.DomainSample.Contracts.Commands
{
    public class DefineProduct : IAmACommandMessage
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
