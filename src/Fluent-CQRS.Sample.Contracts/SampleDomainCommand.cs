using System;

namespace Fluent_CQRS.Sample.Contracts
{
    public class SampleDomainCommand : IAmACommandMessage
    {
        public string Id
        {
            get;
            set;
        }

        public string MyValue { get; set; }
    }
}
