using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class SwitchStep : IBusinessStep
    {
        private readonly IReadOnlyList<(ICondition Condition, IBusinessStep Step)> _cases;
        private readonly string _name;
        private readonly IBusinessStep? _defaultStep;
        private readonly ILogger<SwitchStep>? _logger;

        public SwitchStep(
            string name,
            IEnumerable<(ICondition condition, IBusinessStep step)> cases,
            IBusinessStep? defaultStep = null,
            ILogger<SwitchStep>? logger = null)
        {
            _cases = cases.ToList();
            _name = name;
            _defaultStep = defaultStep;
            _logger = logger;
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            await Parallel.ForEachAsync(_cases,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 5
                },
                async (_case, cancellationToken) =>
                {
                    if (_case.Condition.Evaluate(context))
                    {
                        await _case.Step.ExecuteAsync(context, ct);
                    }
                });
            if (_defaultStep != null)
            {
                await _defaultStep.ExecuteAsync(context, ct);
            }
        }
    }
}
