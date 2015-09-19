namespace Fluent_CQRS.Tests.Infrastructure
{
    public class TestCommand : IAmACommandMessage
    {
        public string Id { get; set; }
    }
}