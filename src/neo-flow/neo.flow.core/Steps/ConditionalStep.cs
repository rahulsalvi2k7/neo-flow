using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class ConditionalStep : IBusinessStep
    {
        private readonly string _name;
        private readonly ICondition _condition;
        private readonly IBusinessStep _thenStep;
        private readonly IBusinessStep? _elseStep;
        private readonly ILogger<ConditionalStep>? _logger;

        public ConditionalStep(
            string name,
            ICondition condition,
            IBusinessStep thenStep,
            IBusinessStep? elseStep = null,
            ILogger<ConditionalStep>? logger = null)
        {
            _condition = condition;
            _thenStep = thenStep;
            _elseStep = elseStep;
            _logger = logger;
            _name = name;
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _logger);

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
