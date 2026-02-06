using neo.flow.core.Interfaces;

namespace neo.flow.core.Base
{
    public abstract class WorkflowBase : IWorkflow
    {
        public abstract IReadOnlyList<IBusinessStep> Steps { get; }

        public virtual string Name => GetType().Name;

        public async Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
        {
            foreach (var step in Steps)
            {
                await step.ExecuteAsync(context, ct);
            }
        }
    }
}
