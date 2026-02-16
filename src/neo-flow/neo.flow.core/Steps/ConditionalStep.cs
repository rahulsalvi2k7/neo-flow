using neo.flow.core.Interfaces;
using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Loggers;

namespace neo.flow.core.Steps
{
    public sealed class ConditionalStep : IBusinessStep
    {
        private readonly ICondition _condition;
        private readonly IBusinessStep _thenStep;
        private readonly IBusinessStep? _elseStep;
        private readonly ILogger _logger;

        public ConditionalStep(
            ICondition condition,
            IBusinessStep thenStep,
            IBusinessStep? elseStep = null,
            ILogger? logger = null)
        {
            _condition = condition;
            _thenStep = thenStep;
            _elseStep = elseStep;
            _logger = logger ?? new ConditionalStepSvgLogger("workflow.svg");
        }

        public string Name => "Conditional";

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, Name, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            if (_condition.Evaluate(context))
            {
                await _thenStep.ExecuteAsync(context, ct);
            }
            else if (_elseStep != null)
            {
                await _elseStep.ExecuteAsync(context, ct);
            }
        }
    }

}
