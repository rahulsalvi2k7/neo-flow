using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.logger.file.text
{
    public class HttpStepTextLogger : ILogger<HttpStep>
    {
        private readonly string _logFilePath;

        public HttpStepTextLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public Task LogExecutionAsync(HttpStep t, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            var resp = context.Get<string>("lastHttpResponse");
            var entry = $"{dateTimeProvider.UtcNow():s} {t.Name} {resp}";
            return File.AppendAllTextAsync(_logFilePath, entry + System.Environment.NewLine);
        }
    }
}
