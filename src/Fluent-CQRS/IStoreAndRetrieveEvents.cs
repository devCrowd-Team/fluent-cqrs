using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
    public interface IStoreAndRetrieveEvents
    {
        void StoreFor(String aggegateId, ICarryEventInformation information);
        IEnumerable<ICarryEventInformation> RetrieveFor(String aggregateId);
    }
}