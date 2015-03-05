using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
    public interface IStoreAndRetrieveEvents
    {
        void StoreFor(String aggegateId, IAmAnEventMessage eventMessage);
        IEnumerable<IAmAnEventMessage> RetrieveFor(String aggregateId);
    }
}