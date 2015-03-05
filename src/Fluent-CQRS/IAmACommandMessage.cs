using System;

namespace Fluent_CQRS
{
    public interface IAmACommandMessage
    {
        String Id { get; set;  }
    }
}