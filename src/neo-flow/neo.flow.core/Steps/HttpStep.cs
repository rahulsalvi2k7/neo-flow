using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;
using System.Text;

namespace neo.flow.core.Steps
{
    public sealed class HttpStep(string name, System.Uri uri, HttpMethod method, string? content = null, HttpClient? httpClient = null, ILogger<HttpStep>? logger = null) : IBusinessStep
    {
        public string Name => _name;

        private readonly string _name = name;
        private readonly System.Uri _uri = uri;
        private readonly HttpMethod _method = method;
        private readonly string? _content = content;
        private readonly HttpClient? _httpClient = httpClient;
        private readonly ILogger<HttpStep>? _logger = logger;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken ct)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, ct, this, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken ct)
        {
            using var client = _httpClient ?? new HttpClient();

            using var req = new HttpRequestMessage(_method, _uri);
            if (_content is not null)
            {
                req.Content = new StringContent(_content, Encoding.UTF8, "application/json");
            }

            using var resp = await client.SendAsync(req, ct).ConfigureAwait(false);
            var responseBody = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            // Store response content and status code in the execution context for downstream steps/loggers
            await context.Set("lastHttpResponse", responseBody, _name).ConfigureAwait(false);
            await context.Set("lastHttpStatusCode", (int)resp.StatusCode, _name).ConfigureAwait(false);
        }
    }
}
