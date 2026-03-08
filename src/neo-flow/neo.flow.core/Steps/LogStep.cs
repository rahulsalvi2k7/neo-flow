using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    public sealed class LogStep(string name, ILogger<LogStep>? logger = null) : IBusinessStep
    {
        public string Name => _name;

        private readonly string _name = name;

        public async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            Console.WriteLine($"{DateTime.UtcNow:s} : {_name}");

            await Task.CompletedTask;
        }
    }
}
