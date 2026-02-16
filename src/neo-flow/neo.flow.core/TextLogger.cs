using System;
using System.IO;
using System.Threading.Tasks;
using neo.flow.core.Engine;
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

        public async Task LogExecutionAsync(string stepName, DateTime startTime, DateTime endTime, Engine.ExecutionContext context)
        {
            var logEntry = $"Step: {stepName}, Start: {startTime:O}, End: {endTime:O}, Duration: {(endTime-startTime).TotalMilliseconds}ms, Context: {SerializeContext(context)}";
            await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
        }

        private string SerializeContext(Engine.ExecutionContext context)
        {
            // For demonstration, just return the count of keys. Customize as needed.
            return $"Keys: {context.GetAuditTrail().Result.Count}";
        }
    }
}
