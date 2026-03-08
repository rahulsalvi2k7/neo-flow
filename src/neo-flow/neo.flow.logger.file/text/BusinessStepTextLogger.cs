using neo.flow.core.Interfaces;

namespace neo.flow.logger.file.text
{
    public class BusinessStepTextLogger : ILogger<IBusinessStep>
    {
        private readonly string _logFilePath;

        public BusinessStepTextLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public Task LogExecutionAsync(IBusinessStep t, IExecutionContext context)
        {
            var entry = $"{context.DateTimeProvider.UtcNow():s} {t.GetType().Name} {t.Name}";

            return File.AppendAllTextAsync(_logFilePath, entry + System.Environment.NewLine);
        }
    }
}
