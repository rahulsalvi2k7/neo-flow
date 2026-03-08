using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Tests.Steps
{
    public class ScriptStepTests
    {
        private Mock<IDateTimeProvider> dateTimeProvider = new Mock<IDateTimeProvider>();
        private Mock<ILogger<ScriptStep>> mockLogger = new Mock<ILogger<ScriptStep>>();

        [Test]
        public async Task ScriptStep_CanGetAndSetContextValues()
        {
            // Arrange            
            dateTimeProvider
            .Setup(d => d.UtcNow())
            .Returns(System.DateTime.UtcNow);


            var context = new Engine.ExecutionContext(dateTimeProvider.Object);
            await context.Set("foo", 42d);
            var script = @"set('bar', get('foo') + 1);";
            var step = new ScriptStep(string.Empty, script);

            // Act
            await step.ExecuteCoreAsync(context, CancellationToken.None);

            // Assert
            var result = context.Get<double>("bar");
            Assert.That(result, Is.EqualTo(43));
        }
    }
}
