using neo.flow.core.Interfaces;
using neo.flow.core.Steps;
using neo.flow.logger.file.text;

namespace neo.flow.logger.file.tests
{
    [TestFixture]
    public class HttpStepTextLoggerTests
    {
        private static string NewTempFilePath() => Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".log");

        private class FakeDateTimeProvider : IDateTimeProvider
        {
            private readonly DateTime _now;
            public FakeDateTimeProvider(DateTime now) => _now = now;
            public DateTime UtcNow() => _now;
        }

        [Test]
        public async System.Threading.Tasks.Task HttpStepTextLogger_WritesNameAndResponse()
        {
            var path = NewTempFilePath();
            try
            {
                var now = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var dtp = new FakeDateTimeProvider(now);
                var ctx = new core.Engine.ExecutionContext(dtp);

                var logger = new HttpStepTextLogger(path);
                var step = new HttpStep("MyHttp", new System.Uri("http://example.invalid/"), System.Net.Http.HttpMethod.Get);

                await ctx.Set("lastHttpResponse", "resp-body");

                await logger.LogExecutionAsync(step, dtp, ctx);

                var content = File.ReadAllText(path);
                Assert.That(content, Does.Contain(step.Name));
                Assert.That(content, Does.Contain(now.ToString("s")));
                Assert.That(content, Does.Contain("resp-body"));
            }
            finally { File.Delete(path); }
        }
    }
}
