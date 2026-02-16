using neo.flow.core.Builder;
using neo.flow.core.Engine;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;

namespace neo.flow.core.Tests.Steps
{
    public class ScriptStepTests
    {
        private Mock<IDateTimeProvider> dateTimeProvider = new Mock<IDateTimeProvider>();

        [Test]
        public async Task ScriptStep_CanGetAndSetContextValues()
        {
            // Arrange            
            dateTimeProvider
            .Setup(d => d.UtcNow())
            .Returns(System.DateTime.UtcNow);

            var context = new Engine.ExecutionContext(dateTimeProvider.Object);
            await context.Set("foo", 42);
            var script = @"set('bar', get('foo') + 1);";
            var step = new ScriptStep(string.Empty, script);

            // Act
            await step.ExecuteAsync(context, CancellationToken.None);

            // Assert
            var result = context.Get<int>("bar");
            Assert.That(result, Is.EqualTo(43));
        }
    }
}
