namespace Fluent_CQRS.Sample.Contracts
{
    public class SampleEventRaised : IAmAnEventMessage
    {
        public string MyValue { get; set; }
    }
}