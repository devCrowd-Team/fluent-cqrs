namespace Fluent_CQRS
{
    public interface IHandle<TMessage>
    {
        void Receive(TMessage message);
    }
}
