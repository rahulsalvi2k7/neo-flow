using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.logger.file.text
{
    public class ScriptStepTextLogger : ILogger<ScriptStep>
    {
        private readonly string _logFilePath;

        public ScriptStepTextLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public Task LogExecutionAsync(ScriptStep t, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            var entry = $"{dateTimeProvider.UtcNow():s} {t.Name}";
            return File.AppendAllTextAsync(_logFilePath, entry + System.Environment.NewLine);
        }
    }
}
