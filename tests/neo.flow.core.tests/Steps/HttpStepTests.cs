using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Tests.Steps
{
    public class HttpStepTests
    {
        private Mock<IDateTimeProvider> mockDateTimeProvider = null!;

        [SetUp]
        public void Setup()
        {
            mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(m => m.UtcNow()).Returns(System.DateTime.UtcNow);
        }

        private class FakeHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _resp;
            public FakeHandler(HttpResponseMessage resp) => _resp = resp;
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(_resp);
        }

        [Test]
        public async Task HttpStep_StoresResponseInContext()
        {
            var ctx = new core.Engine.ExecutionContext(mockDateTimeProvider.Object);

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("hello world")
            };

            var client = new HttpClient(new FakeHandler(response));

            var step = new HttpStep("MyHttp", new System.Uri("http://example.invalid/"), HttpMethod.Get, null, client);

            await step.ExecuteAsync(ctx, CancellationToken.None);

            var resp = ctx.Get<string>("lastHttpResponse");
            var status = ctx.Get<int>("lastHttpStatusCode");

            Assert.That(resp, Is.EqualTo("hello world"));
            Assert.That(status, Is.EqualTo((int)System.Net.HttpStatusCode.OK));
        }
    }
}
