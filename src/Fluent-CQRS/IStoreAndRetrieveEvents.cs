using System.Collections.Generic;

namespace Fluent_CQRS
{
    public interface IStoreAndRetrieveEvents
    {
        void StoreFor<TAggregate>(string Id, IAmAnEventMessage eventMessage) where TAggregate : Aggregate;
        
        IEnumerable<IAmAnEventMessage> RetrieveFor(string aggregateId);
        IEnumerable<IAmAnEventMessage> RetrieveFor<TAggregate>(string aggregateId) where TAggregate : Aggregate;
        IEnumerable<IAmAnEventMessage> RetrieveFor<TAggregate>() where TAggregate : Aggregate;
    }
}