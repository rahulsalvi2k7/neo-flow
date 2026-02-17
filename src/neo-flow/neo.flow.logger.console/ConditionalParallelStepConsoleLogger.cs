using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.logger.Console
{
    public class ConditionalParallelStepConsoleLogger : ILogger<ConditionalParallelStep>
    {
        public Task LogExecutionAsync(ConditionalParallelStep t, Engine.ExecutionContext context)
        {
            System.Console.WriteLine($"{DateTime.UtcNow:s} {t.Name}");

            return Task.CompletedTask;
        }
    }
}
