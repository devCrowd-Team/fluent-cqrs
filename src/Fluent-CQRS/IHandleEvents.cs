using System;
using System.Collections.Generic;

namespace Fluent_CQRS
{
    [Obsolete("Please use the generic IHandle interface instead")]
    public interface IHandleEvents:IHandle<IEnumerable<IAmAnEventMessage>>
    {   
    }
}

