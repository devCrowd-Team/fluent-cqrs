using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS
{
    [Obsolete("Please use AggregateContext instead.")]
    public sealed class Aggregates : AggregateContext
    {
        internal Aggregates(IStoreAndRetrieveEvents eventStore) : base(eventStore) { }

        public static new Aggregates CreateWith(IStoreAndRetrieveEvents eventStore)
        {
            return new Aggregates(eventStore);
        }
    }
}
