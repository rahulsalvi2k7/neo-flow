using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class StartStep(string name, ILogger<StartStep>? logger = null) : IBusinessStep
    {
        public string Name => _name;

        private readonly string _name = name;
        private readonly ILogger<StartStep>? _logger = logger;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            await Task.CompletedTask;
        }
    }
}
