using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.logger.Console;
using neo.flow.core.Steps;

namespace neo.flow.logger.console.tests
{
    [TestFixture]
    public class ScriptStepConsoleLoggerTests
    {
        private Mock<IDateTimeProvider> mockDateTimeProvider = null!;
        private core.Engine.ExecutionContext execContext = null!;

        [SetUp]
        public void Setup()
        {
            mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(m => m.UtcNow()).Returns(DateTime.UtcNow);
            execContext = new core.Engine.ExecutionContext(mockDateTimeProvider.Object);
        }

        [Test]
        public async System.Threading.Tasks.Task LogExecutionAsync_WritesStepNameToConsole()
        {
            var logger = new ScriptStepConsoleLogger();
            var step = new ScriptStep("MyScript", "1+1;");

            using var sw = new StringWriter();
            var original = Console.Out;
            try
            {
                Console.SetOut(sw);
                await logger.LogExecutionAsync(step, execContext);
            }
            finally
            {
                Console.SetOut(original);
            }

            var output = sw.ToString();
            Assert.That(output, Does.Contain(step.Name));
        }
    }
}
