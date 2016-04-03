using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent_CQRS
{
    public struct CorrelationId
    {
        readonly string _sagaId;

        public CorrelationId(string sagaId)
        {
            _sagaId = sagaId;
        }

        public string Value { get { return _sagaId; } }

    }
}
