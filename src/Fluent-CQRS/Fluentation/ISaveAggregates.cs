namespace Fluent_CQRS.Fluentation
{
    public interface ISaveAggregates
    {
        IPublishNewState FinallySaveIt();
    }
}