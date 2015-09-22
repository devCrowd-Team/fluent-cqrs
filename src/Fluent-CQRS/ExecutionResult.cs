using System;

namespace Fluent_CQRS
{
    public class ExecutionResult
    {
        public Exception Exception { get; set; }

        public Fault Fault { get; set; }

        public bool Executed { get; set; }

        public bool Saved { get; set; }

        public bool Published { get; set; }
    }
}