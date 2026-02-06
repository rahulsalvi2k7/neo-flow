using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class ParallelStep : IBusinessStep
    {
        private readonly string _name;
        private readonly IReadOnlyList<IBusinessStep> _steps;

        public ParallelStep(string name, params IBusinessStep[] steps)
        {
            _name = name;
            _steps = steps;
        }

        public string Name => _name;

        public async Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
        {
            await Task.WhenAll(
                _steps.Select(s => s.ExecuteAsync(context, ct))
            );
        }
    }
}
