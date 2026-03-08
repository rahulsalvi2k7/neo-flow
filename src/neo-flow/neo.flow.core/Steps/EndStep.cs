using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class EndStep(string name) : IBusinessStep
    {
        public string Name => _name;

        private readonly string _name = name;

        public async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            await Task.CompletedTask;

            return;
        }
    }
}
