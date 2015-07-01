using System;
using Fluent_CQRS.Sample.Contracts;

namespace Fluent_CQRS.Sample.Domain
{
    public class SampleEventHandler
    {
        public void HandleMessage(SampleEventRaised message)
        {
            Console.WriteLine("Event {0} empfangen: {1}", message.GetType(),message.MyValue);
        }
    }
}
