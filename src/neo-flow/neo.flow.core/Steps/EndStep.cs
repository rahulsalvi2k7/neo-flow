using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class EndStep(string name, ILogger? logger = null) : IBusinessStep
    {
        public string Name => _name;

        private readonly string _name = name;
        private readonly ILogger? _logger = logger;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
    => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, Name, _logger);

        public async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            await Task.CompletedTask;

            return;
        }
    }
}
