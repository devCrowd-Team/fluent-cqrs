using System;

namespace Fluent_CQRS
{
    /// <summary>
    /// This class have to use for issues and/or faults in the business logic, not for Exceptions
    /// </summary>
    public abstract class Fault : Exception
    {
        protected Fault(string message) : base(message)
        {
            
        }
    }
}