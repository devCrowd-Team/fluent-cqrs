using System;
using System.Collections.Generic;

namespace Fluent_CQRS.Extensions
{
    public static class AggregateStoreExtension
    {
        public static bool IsNotDefined(this Action<IEnumerable<ICarryEventInformation>> source)
        {
            return source == null;
        } 
    }
}