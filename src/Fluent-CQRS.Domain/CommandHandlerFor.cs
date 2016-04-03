namespace Fluent_CQRS
{
    public abstract class CommandHandlerFor<TAggregate> : Handler<IAmACommandMessage> where TAggregate : Aggregate
    {
        private AggregateContext _aggregateContext;

        public CommandHandlerFor(AggregateContext aggregateContext)
        {
            _aggregateContext = aggregateContext;
        }

        public IInvokeActionsOnAggregates<TAggregate> With(IAmACommandMessage cmd)
        {
            return _aggregateContext.Provide<TAggregate>().With(cmd);
        }
    }
}
