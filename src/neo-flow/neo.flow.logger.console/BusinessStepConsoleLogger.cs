using neo.flow.core.Interfaces;

namespace neo.flow.core.logger.Console
{
    public class BusinessStepConsoleLogger : ILogger<IBusinessStep>
    {
        public Task LogExecutionAsync(IBusinessStep t, IExecutionContext context)
        {
            System.Console.WriteLine($"{context.DateTimeProvider.UtcNow():s} {t.GetType().Name} {t.Name}");

            return Task.CompletedTask;
        }
    }
}
