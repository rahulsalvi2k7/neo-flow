using System.Threading;
using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;
using NUnit.Framework;

namespace neo.flow.core.tests.Steps
{
    [TestFixture]
    public class ConditionalStepTests
    {
        private Mock<ICondition> mockCondition = null!;
        private Mock<IBusinessStep> mockBusinessStep = null!;
        private Mock<IExecutionContext> mockIExecutionContext = null!;

        [SetUp]
        public void Setup()
        {
            mockCondition = new Mock<ICondition>();
            mockBusinessStep = new Mock<IBusinessStep>();
            mockIExecutionContext = new Mock<IExecutionContext>();
        }

        [Test]
        public async Task ExecuteAsync_ConditionTrue_ThenStepExecuted()
        {
            // Arrange
            mockCondition
                .Setup(m => m.Evaluate(It.IsAny<IExecutionContext>()))
                .Returns(true);

            var conditionalStep = new ConditionalStep(mockCondition.Object, mockBusinessStep.Object);

            // Act
            await conditionalStep.ExecuteAsync(mockIExecutionContext.Object, CancellationToken.None);

            // Assert
            mockBusinessStep.Verify(m => m.ExecuteAsync(mockIExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_ConditionFalse_WithElse_ElseStepExecuted()
        {
            // Arrange
            mockCondition
                .Setup(m => m.Evaluate(It.IsAny<IExecutionContext>()))
                .Returns(false);

            var mockElseStep = new Mock<IBusinessStep>();
            var conditionalStep = new ConditionalStep(mockCondition.Object, mockBusinessStep.Object, mockElseStep.Object);

            // Act
            await conditionalStep.ExecuteAsync(mockIExecutionContext.Object, CancellationToken.None);

            // Assert
            mockElseStep.Verify(m => m.ExecuteAsync(mockIExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockBusinessStep.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_ConditionFalse_NoElse_NoStepExecuted()
        {
            // Arrange
            mockCondition
                .Setup(m => m.Evaluate(It.IsAny<IExecutionContext>()))
                .Returns(false);

            var conditionalStep = new ConditionalStep(mockCondition.Object, mockBusinessStep.Object);

            // Act
            await conditionalStep.ExecuteAsync(mockIExecutionContext.Object, CancellationToken.None);

            // Assert
            mockBusinessStep.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void Name_ReturnsConditional()
        {
            // Arrange
            var conditionalStep = new ConditionalStep(Mock.Of<ICondition>(), Mock.Of<IBusinessStep>());

            // Act / Assert
            Assert.That(conditionalStep.Name, Is.EqualTo("Conditional"));
        }
    }
}
