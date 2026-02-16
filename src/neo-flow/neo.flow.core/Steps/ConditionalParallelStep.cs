using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Loggers;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class ConditionalParallelStep : IBusinessStep
    {
        private readonly IReadOnlyList<(ICondition Condition, IBusinessStep Step)> _branches;
        private readonly string _name;
        private readonly IBusinessStep? _defaultStep;
        private readonly ILogger _logger;


        public ConditionalParallelStep(
            string name,
            IEnumerable<(ICondition condition, IBusinessStep step)> branches,
            IBusinessStep? defaultStep = null,
            ILogger? logger = null)
        {
            _branches = branches.ToList();
            _name = name;
            _defaultStep = defaultStep;
            _logger = logger ?? new ConditionalParallelStepSvgLogger("workflow.svg");
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, Name, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            var tasks = new List<Task>();

            foreach (var (condition, step) in _branches)
            {
                if (condition.Evaluate(context))
                {
                    tasks.Add(step.ExecuteAsync(context, ct));
                }
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
            else if (_defaultStep != null)
            {
                await _defaultStep.ExecuteAsync(context, ct);
            }
        }
    }
}
