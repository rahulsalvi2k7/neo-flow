using neo.flow.core.Interfaces;

namespace neo.flow.core
{
    /// <summary>
    /// Simple text logger implementation for workflow steps.
    /// </summary>
    public class TextLogger : ILogger
    {
        private readonly string _logFilePath;

        public TextLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public async Task LogExecutionAsync(string stepName, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            var timestamp = dateTimeProvider.UtcNow();
            var logEntry = $"Step: {stepName}, Timestamp: {timestamp:O}, Context: {SerializeContext(context)}";
            await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
        }

        private string SerializeContext(IExecutionContext context)
        {
            // For demonstration, just return the count of keys. Customize as needed.
            return $"Keys: {context.GetAuditTrail().Result.Count}";
        }
    }
}
