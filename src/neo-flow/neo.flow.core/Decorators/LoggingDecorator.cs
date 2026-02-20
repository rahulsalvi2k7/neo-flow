using neo.flow.core.Interfaces;

namespace neo.flow.core.Decorators
{
    /// <summary>
    /// Decorator to intercept ExecuteAsync methods with LogExecutionAttribute.
    /// </summary>
    public static class LoggingDecorator
    {
        public static async Task InvokeWithLoggingAsync<T>(
            Func<IExecutionContext, CancellationToken, Task> method,
            IExecutionContext context,
            CancellationToken ct,
            T t,
            ILogger<T>? logger)
        {

            try
            {
                await method(context, ct);
            }
            finally
            {
                if (context is Engine.ExecutionContext execContext)
                {
                    if (logger is not null)
                    {
                        await logger.LogExecutionAsync(t, execContext.DateTimeProvider, execContext);
                    }
                }
            }
        }
    }
}
