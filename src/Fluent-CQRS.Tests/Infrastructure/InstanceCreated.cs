namespace Fluent_CQRS.Tests.Infrastructure
{
    public class InstanceCreated : IAmAnEventMessage
    {
        public string Id { get; set; }
    }
}