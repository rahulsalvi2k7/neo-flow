using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.logger.Console
{
    public class ScriptStepConsoleLogger : ILogger<ScriptStep>
    {
        public Task LogExecutionAsync(ScriptStep t, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            System.Console.WriteLine($"{dateTimeProvider.UtcNow():s} {t.Name}");

            return Task.CompletedTask;
        }
    }
}
