using Fluent_CQRS.Tests.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS.Tests
{
    public class With_AggregateUnderTest
    {
        internal AggregateUnderTest _aggregate;


        [SetUp]
        public void Setup()
        {
            var events = new IAmAnEventMessage[]
             {
                new ValueInitialized {Value = 1 },
                new ValueChanged {Value = 2},
                new ValueChanged {Value = 3},
                new SomeThingElseHappened(),
                new ValueChanged {Value = 4},
                new ValueChanged {Value = 5}
             };

            _aggregate = new AggregateUnderTest("", events);
        }
    }
}
