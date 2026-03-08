using neo.flow.core.Attributes;
using neo.flow.core.Decorators;

namespace neo.flow.core.Interfaces
{
    public interface IBusinessStep
    {
        string Name { get; }

        [LogExecution]
        [LogDbExecution]
        async Task ExecuteAsync(
            IExecutionContext context,
            CancellationToken ct,
            ILogger<IBusinessStep> _logger = null,
            IDbLogger<IBusinessStep> _dbLogger = null)
        {
            await Task.WhenAll(
                LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _logger),
                LoggingDbDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _dbLogger)
            );
        }

        Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct);
    }
}
