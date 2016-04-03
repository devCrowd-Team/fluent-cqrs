﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS
{
    interface IReceive<TMessage>
    {
        void Tell(TMessage message);
    }
}
