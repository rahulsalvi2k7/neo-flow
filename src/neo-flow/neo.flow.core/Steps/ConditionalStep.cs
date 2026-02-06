using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class ConditionalStep : IBusinessStep
    {
        private readonly ICondition _condition;
        private readonly IBusinessStep _thenStep;
        private readonly IBusinessStep? _elseStep;

        public ConditionalStep(
            ICondition condition,
            IBusinessStep thenStep,
            IBusinessStep? elseStep = null)
        {
            _condition = condition;
            _thenStep = thenStep;
            _elseStep = elseStep;
        }

        public string Name => "Conditional";

        public async Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
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
