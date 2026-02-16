using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class StartStep(string name, IBusinessStep next, ILogger? logger = null) : IBusinessStep
    {
        public string Name => _name;

        private readonly string _name = name;
        private readonly IBusinessStep? _next = next;
        private readonly ILogger? _logger = logger;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, Name, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            if (_next is null)
            {
                await Task.CompletedTask;

                return;
            }

            await _next.ExecuteAsync(context, ct);
        }
    }
}
