namespace Fluent_CQRS.Fluentation
{
    public interface IProvideAnAggregate<TAggregate>
    {
        IInvokeActionsOnAggregates<TAggregate> With(IAmACommandMessage message);
    }
}