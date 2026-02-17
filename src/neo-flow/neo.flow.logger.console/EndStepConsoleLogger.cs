using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.logger.Console
{
    public class EndStepConsoleLogger : ILogger<EndStep>
    {
        public Task LogExecutionAsync(EndStep t, Engine.ExecutionContext context)
        {
            System.Console.WriteLine($"{DateTime.UtcNow:s} {t.Name}");

            return Task.CompletedTask;
        }
    }
}
