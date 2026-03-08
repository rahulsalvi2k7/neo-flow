using neo.flow.core.Interfaces;

namespace neo.flow.core.Decorators
{
    /// <summary>
    /// Decorator to intercept ExecuteAsync methods with LogExecutionAttribute.
    /// </summary>
    public static class LoggingDbDecorator
    {
        public static async Task InvokeWithLoggingAsync<T>(
            Func<IExecutionContext, CancellationToken, Task> method,
            IExecutionContext context,
            CancellationToken ct,
            T t,
            IDbLogger<T>? logger) where T : IBusinessStep
        {

            await logger?.LogStartExecutionAsync(t, context);

            try
            {
                await method(context, ct);
            }
            finally
            {
                await logger?.LogEndExecutionAsync(t, context);
            }
        }
    }
}
