using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class HttpStepBuilder
    {
        private readonly string _name;
        private System.Uri? _uri;
        private HttpMethod _method = HttpMethod.Get;
        private string? _content;
        private HttpClient? _httpClient;
        private ILogger<HttpStep>? _logger;

        public HttpStepBuilder(string name)
        {
            _name = name;
        }

        public HttpStepBuilder WithUri(string uri)
        {
            _uri = new System.Uri(uri);
            return this;
        }

        public HttpStepBuilder WithMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public HttpStepBuilder WithContent(string content)
        {
            _content = content;
            return this;
        }

        public HttpStepBuilder WithHttpClient(HttpClient client)
        {
            _httpClient = client;
            return this;
        }

        public HttpStepBuilder WithLogger(ILogger<HttpStep> logger)
        {
            _logger = logger;
            return this;
        }

        public HttpStep Build()
        {
            if (_uri is null) throw new System.InvalidOperationException("Uri must be provided");
            return new HttpStep(_name, _uri, _method, _content, _httpClient, _logger);
        }
    }
}
