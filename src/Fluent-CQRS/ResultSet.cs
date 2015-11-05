namespace Fluent_CQRS
{
    class ResultSet<TResult>
    {
        public ResultSet(TResult result, bool modified)
        {
            Modified = modified;
            State = result;
        }
        public bool Modified { get; private set; }
        public TResult State { get; private set; }
    }
}
