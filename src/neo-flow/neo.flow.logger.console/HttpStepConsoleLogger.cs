using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.logger.Console
{
    public class HttpStepConsoleLogger : ILogger<HttpStep>
    {
        public async Task LogExecutionAsync(HttpStep t, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            var ts = dateTimeProvider.UtcNow();
            var lastResp = context.Get<string>("lastHttpResponse");
            var shortResp = lastResp is null ? string.Empty : (lastResp.Length > 120 ? lastResp[..120] + "..." : lastResp);

            System.Console.WriteLine($"{ts:s} {t.Name} {shortResp}");
            await Task.CompletedTask;
        }
    }
}
