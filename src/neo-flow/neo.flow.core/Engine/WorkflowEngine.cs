using neo.flow.core.Interfaces;

namespace neo.flow.core.Engine
{
    public sealed class WorkflowEngine
    {
        private readonly IStepObserver? _stepObserver;

        public WorkflowEngine(IStepObserver? stepObserver)
        {
            _stepObserver = stepObserver;
        }

        public async Task RunAsync(
            IWorkflow workflow,
            IExecutionContext context,
            CancellationToken ct = default)
        {
            await _stepObserver?.OnStepStarted(workflow.Name);

            try
            {
                await workflow.ExecuteAsync(context, ct);

                await _stepObserver?.OnStepCompleted(workflow.Name);
            }
            catch (Exception)
            {
                await _stepObserver?.OnStepFailed(workflow.Name);
            }
        }
    }
}
