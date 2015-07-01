using System;

namespace Fluent_CQRS
{

    public struct ExecutionFlags
    {
        public bool WasExecuted { get; set; }
        public bool WasSaved { get; set; }
        public bool WasPublished { get; set; }

        internal ExecutionFlags Saved()
        {
            return new ExecutionFlags
            {
                WasExecuted = this.WasExecuted,
                WasSaved = true,
                WasPublished = this.WasPublished
            };
        }

        public ExecutionFlags Published()
        {
            return new ExecutionFlags
            {
                WasExecuted = this.WasExecuted,
                WasSaved = this.WasPublished,
                WasPublished = true
            };
        }
    }

    public class ExecutionResult<tResult>
    {
        public Exception Error { get; private set; }
        public tResult Result { get; private set; }

        public ExecutionResult(Exception error)
        {
            Error = error;
        }

        public ExecutionResult(tResult result)
        {
            Result = result;
        }

        public ExecutionResult(Exception error, tResult result)
        {
            Error = error;
            Result = result;
        }

        public bool HasError
        {
            get { return Error != null; }
        }
    }

    public static class ExecutionResult
    {
        public static ExecutionResult<tResult> ThenTry<tResult>(this ExecutionResult<tResult> m,
            Func<tResult, tResult> f)
        {
            try
            {
                return new ExecutionResult<tResult>(f(m.Result));
            }
            catch (Exception ex)
            {
                return new ExecutionResult<tResult>(ex, m.Result);
            }
        }

        public static ExecutionResult<tResult> Try<tResult>(this Func<tResult> f)
        {
            try
            {
                return new ExecutionResult<tResult>(f());
            }
            catch (Exception ex)
            {
                return new ExecutionResult<tResult>(ex);
            }
        }


    }
}