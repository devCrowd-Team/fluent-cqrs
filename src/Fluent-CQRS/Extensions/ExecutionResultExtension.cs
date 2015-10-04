using System;

namespace Fluent_CQRS.Extensions
{
    public static class ExecutionResultExtension
    {
        public static void CatchException(this ExecutionResult result, Action<Exception> handle)
        {
            if (result.Exception == null)
                return;

            handle(result.Exception);
        }

        public static ExecutionResult CatchFault(this ExecutionResult result, Action<Fault> handle)
        {
            if (result.Fault != null)
            {
                handle(result.Fault);
            }

            return result;
        }

        public static bool HasErrors(this ExecutionResult result)
        {
            return result.Exception != null || result.Fault != null;
        }
    }
}