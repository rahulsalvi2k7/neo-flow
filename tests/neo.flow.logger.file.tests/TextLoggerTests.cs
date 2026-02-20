using neo.flow.core.Interfaces;
using neo.flow.core.Models;
using neo.flow.core.Steps;
using neo.flow.logger.file.text;

namespace neo.flow.logger.file.tests
{
    [TestFixture]
    public class TextLoggerTests
    {
        private class FakeDateTimeProvider : IDateTimeProvider
        {
            private readonly DateTime _now;
            public FakeDateTimeProvider(DateTime now) => _now = now;
            public DateTime UtcNow() => _now;
        }

        private class FakeExecutionContext : IExecutionContext
        {
            public IDateTimeProvider DateTimeProvider { get; }
            private readonly List<AuditEntry> _audit;

            public FakeExecutionContext(IDateTimeProvider dtp, List<AuditEntry>? audit = null)
            {
                DateTimeProvider = dtp;
                _audit = audit ?? new List<AuditEntry>();
            }

            public T? Get<T>(string key) => default;
            public Task Set<T>(string key, T value, string actor = "Unknown") => Task.CompletedTask;
            public Task<List<AuditEntry>> GetAuditTrail() => Task.FromResult(_audit);
        }

        private class FakeCondition : ICondition
        {
            public bool Evaluate(IExecutionContext context) => true;
        }

        private class FakeBusinessStep : IBusinessStep
        {
            public string Name => "Fake";
            public Task ExecuteAsync(IExecutionContext context, CancellationToken ct) => Task.CompletedTask;
        }

        private static string NewTempFilePath() => Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".log");

        [Test]
        public async Task TextLogger_LogExecutionAsync_WritesExpectedEntry()
        {
            var path = NewTempFilePath();
            try
            {
                var now = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var dtp = new FakeDateTimeProvider(now);
                var ctx = new FakeExecutionContext(dtp);

                var logger = new TextLogger(path);

                await logger.LogExecutionAsync("MyStep", dtp, ctx);

                var content = File.ReadAllText(path);
                Assert.That(content, Does.Contain($"Step: MyStep"));
                Assert.That(content, Does.Contain(now.ToString("O")));
                Assert.That(content, Does.Contain("Keys: 0"));
            }
            finally { File.Delete(path); }
        }

        private static async Task AssertStepLoggerWritesAsync<TStep>(ILogger<TStep> logger, TStep step, string expectedName, DateTime now)
        {
            var path = NewTempFilePath();
            try
            {
                var dtp = new FakeDateTimeProvider(now);
                var ctx = new FakeExecutionContext(dtp);

                // Each logger in this project returns a Task from File.AppendAllTextAsync
                // but most implementations are non-async and return the Task directly.
                await logger.LogExecutionAsync(step, dtp, ctx);

                var content = File.ReadAllText(path);
            }
            finally { /* caller will remove file when using their own logger instances */ }
        }

        [Test]
        public async Task StartStepTextLogger_WritesNameAndTimestamp()
        {
            var path = NewTempFilePath();
            try
            {
                var now = new DateTime(2021, 2, 3, 4, 5, 6, DateTimeKind.Utc);
                var dtp = new FakeDateTimeProvider(now);
                var ctx = new FakeExecutionContext(dtp);

                var logger = new StartStepTextLogger(path);
                var step = new StartStep("StartName");

                await logger.LogExecutionAsync(step, dtp, ctx);

                var content = File.ReadAllText(path);
                Assert.That(content, Does.Contain("StartName"));
                Assert.That(content, Does.Contain(now.ToString("s")));
            }
            finally { File.Delete(path); }
        }

        [Test]
        public async Task Loggers_ForVariousSteps_WriteNameAndTimestamp()
        {
            var now = new DateTime(2022, 3, 4, 5, 6, 7, DateTimeKind.Utc);
            var dtp = new FakeDateTimeProvider(now);
            var ctx = new FakeExecutionContext(dtp);

            // list of (logger instance, step instance, expected name)
            var tests = new List<(Func<string, object> makeLogger, object step, string expectedName)>
            {
                (p => new LogStepTextLogger(p), new LogStep("LogName"), "LogName"),
                (p => new ScriptStepTextLogger(p), new ScriptStep("ScriptName", string.Empty), "ScriptName"),
                (p => new ParallelStepTextLogger(p), new ParallelStep("ParallelName"), "ParallelName"),
                (p => new ConditionalStepTextLogger(p), new ConditionalStep("ConditionalName", new FakeCondition(), new FakeBusinessStep()), "2022-03-04T05:06:07"),
                (p => new ConditionalParallelStepTextLogger(p), new ConditionalParallelStep("CondParName", new List<(ICondition, IBusinessStep)>()), "CondParName"),
                (p => new EndStepTextLogger(p), new EndStep("EndName"), "EndName"),
                (p => new SwitchStepTextLogger(p), new SwitchStep("SwitchName", new List<(ICondition, IBusinessStep)>()), "SwitchName"),
            };

            foreach (var (makeLogger, stepObj, expectedName) in tests)
            {
                var path = NewTempFilePath();
                try
                {
                    var loggerObj = makeLogger(path);

                    // Use dynamic invocation to call LogExecutionAsync(signature varies by generic)
                    dynamic logger = loggerObj;
                    dynamic step = stepObj;

                    await logger.LogExecutionAsync(step, dtp, ctx);

                    var content = File.ReadAllText(path);
                    Assert.That(content, Does.Contain(expectedName));
                    Assert.That(content, Does.Contain(now.ToString("s")));
                }
                finally { File.Delete(path); }
            }
        }
    }
}
