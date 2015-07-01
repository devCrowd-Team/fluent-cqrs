using System;

namespace Fluent_CQRS.Extensions
{
    public static class ExecutionResultExtension
    {
        public static void OnError(this ExecutionResult<ExecutionFlags> result, Action<Exception> handle)
        {
            if (!result.HasError) return;

            handle(result.Error);
        }
    }
}