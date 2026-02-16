using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;
using neo.flow.core.Loggers;

namespace neo.flow.core.Steps
{
    public sealed class ParallelStep : IBusinessStep
    {
        private readonly string _name;
        private readonly IReadOnlyList<IBusinessStep> _steps;
        private readonly ILogger _logger;

        public ParallelStep(string name, ILogger? logger = null, params IBusinessStep[] steps)
        {
            _name = name;
            _steps = steps;
            _logger = logger ?? new ParallelStepSvgLogger("workflow.svg");
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, Name, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            await Task.WhenAll(
                _steps.Select(s => s.ExecuteAsync(context, ct))
            );
        }
    }
}
