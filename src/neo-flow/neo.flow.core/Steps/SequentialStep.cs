using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class SequentialStep : IBusinessStep
    {
        private readonly string _name;
        private readonly IReadOnlyList<IBusinessStep> _steps;

        public SequentialStep(string name, params IBusinessStep[] steps)
        {
            _name = name;
            _steps = steps;
        }

        public string Name => _name;

        public async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            foreach (var step in _steps)
            {
                await step.ExecuteAsync(context, ct);
            }
        }
    }
}
