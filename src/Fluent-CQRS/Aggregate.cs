using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
    public class Aggregate
    {
        public Aggregate(String id)
        {
            Changes = new List<ICarryEventInformation>();
            History = new List<ICarryEventInformation>();

            Id = id;
        }

        public IEnumerable<ICarryEventInformation> History { get; set; }

        public IList<ICarryEventInformation> Changes { get; private set; }

        public String Id { get; protected internal set; }
    }
}