using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class SequentialStep : IBusinessStep
    {
        private readonly string _name;
        private readonly IReadOnlyList<IBusinessStep> _steps;
        private readonly ILogger<SequentialStep>? _logger;

        public SequentialStep(string name, ILogger<SequentialStep>? logger = null, params IBusinessStep[] steps)
        {
            _name = name;
            _steps = steps;
            _logger = logger;
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            foreach (var step in _steps)
            {
                await step.ExecuteAsync(context, ct);
            }
        }
    }
}
