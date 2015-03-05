using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
    public class Aggregate
    {
        public Aggregate(String id)
        {
            Changes = new List<IAmAnEventMessage>();
            History = new List<IAmAnEventMessage>();

            Id = id;
        }

        public IEnumerable<IAmAnEventMessage> History { get; set; }

        public IList<IAmAnEventMessage> Changes { get; private set; }

        public String Id { get; protected internal set; }
    }
}