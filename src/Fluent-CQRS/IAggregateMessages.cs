using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS
{
    public interface IAggregateMessages<TResult>
    {
        TResult AggregateAllMessages();
    }
}
