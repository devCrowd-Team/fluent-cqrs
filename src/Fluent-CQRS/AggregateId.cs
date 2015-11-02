using System;

namespace Fluent_CQRS
{
    public struct AggregateId
    {
        readonly string _aggregateId;

        public AggregateId(string aggregateId)
        {
            _aggregateId = aggregateId;
        }

        public string Value { get { return _aggregateId; } }
    }
}