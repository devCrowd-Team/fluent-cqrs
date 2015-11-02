using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.Tests.Infrastructure
{
    class ValueInitialized : IAmAnEventMessage
    {
        public int Value { get; set; }
    }
}
