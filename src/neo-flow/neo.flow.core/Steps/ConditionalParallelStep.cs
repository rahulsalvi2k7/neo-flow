using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class ConditionalParallelStep : IBusinessStep
    {
        private readonly IReadOnlyList<(ICondition Condition, IBusinessStep Step)> _branches;
        private readonly string _name;
        private readonly IBusinessStep? _defaultStep;

        public ConditionalParallelStep(
            string name,
            IEnumerable<(ICondition condition, IBusinessStep step)> branches,
            IBusinessStep? defaultStep = null)
        {
            _branches = branches.ToList();
            _name = name;
            _defaultStep = defaultStep;
        }

        public string Name => _name;

        public async Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
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
