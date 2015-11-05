namespace Fluent_CQRS
{
    class AggregationState<TResult>
    {
        public AggregationState(bool applied, TResult result)
        {
            Applied = applied;
            Result = result;
        }
        public bool Applied { get; private set; }
        public TResult Result { get; private set; }
    }

}
