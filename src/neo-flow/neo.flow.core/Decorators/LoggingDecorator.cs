using neo.flow.core.Interfaces;

namespace neo.flow.core.Decorators
{
    /// <summary>
    /// Decorator to intercept ExecuteAsync methods with LogExecutionAttribute.
    /// </summary>
    public static class LoggingDecorator
    {
        public static async Task InvokeWithLoggingAsync(
            Func<IExecutionContext, CancellationToken, Task> method,
            IExecutionContext context,
            CancellationToken ct,
            string stepName,
            ILogger? logger)
        {
            var start = DateTime.UtcNow;
            try
            {
                await method(context, ct);
            }
            finally
            {
                var end = DateTime.UtcNow;

                if (context is Engine.ExecutionContext execContext)
                {
                    if (logger is not null)
                    {
                        await logger.LogExecutionAsync(stepName, start, end, execContext);
                    }
                }
            }
        }
    }
}
