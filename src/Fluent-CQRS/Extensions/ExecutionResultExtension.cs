using System;

namespace Fluent_CQRS.Extensions
{
    public static class ExecutionResultExtension
    {
        public static void OnError(this ExecutionResult result, Action<Exception> handle)
        {
            if (result.Error == null) return;

            handle(result.Error);
        }
    }
}