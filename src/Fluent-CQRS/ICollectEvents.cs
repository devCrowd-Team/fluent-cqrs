namespace Fluent_CQRS
{
    public interface ICollectEvents
    {
        IReplayEvents EventsWithAggregateId(string aggregateId);
        IReplayEvents AllEvents();
    }
}