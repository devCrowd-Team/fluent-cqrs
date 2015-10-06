namespace Fluent_CQRS.Tests.Infrastructure
{
    public class SomethingSpecialHappend : IAmAnEventMessage
    {
        public string NiceProperty { get; set; }
    }
}