namespace Fluent_CQRS
{
    public interface ICollectEvents
    {
        IReplayEvents WithId(string aggregateId);
        IReplayEvents All();
    }
}