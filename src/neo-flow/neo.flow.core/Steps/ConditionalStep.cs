using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class ConditionalStep : IBusinessStep
    {
        private readonly string _name;
        private readonly ICondition _condition;
        private readonly IBusinessStep _thenStep;
        private readonly IBusinessStep? _elseStep;

        public ConditionalStep(
            string name,
            ICondition condition,
            IBusinessStep thenStep,
            IBusinessStep? elseStep = null)
        {
            _condition = condition;
            _thenStep = thenStep;
            _elseStep = elseStep;
            _name = name;
        }

        public string Name => _name;

        public async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
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
