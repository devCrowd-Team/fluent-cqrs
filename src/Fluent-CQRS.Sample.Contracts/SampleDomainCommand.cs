using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
