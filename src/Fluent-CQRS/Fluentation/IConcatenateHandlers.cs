using System.Collections.Generic;

namespace Fluent_CQRS.Fluentation
{
    public interface IConcatenateHandlers<T>
    {
        IConcatenateHandlers<T> And(IHandle<T> eventHandler);
        IConcatenateHandlers<T> And(IHandle<IEnumerable<T>> eventHandler);
    }
}