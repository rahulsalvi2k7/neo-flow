using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.tests.Steps
{

    [TestFixture]
    public class ParallelStepTests
    {
        private Mock<IExecutionContext> mockExecutionContext = null!;

        [SetUp]
        public void Setup()
        {
            mockExecutionContext = new Mock<IExecutionContext>();
        }

        [Test]
        public async Task ExecuteAsync_WithMultipleSteps_AllStepsExecuted()
        {
            // Arrange
            var mockStep1 = new Mock<IBusinessStep>();
            var mockStep2 = new Mock<IBusinessStep>();
            var mockStep3 = new Mock<IBusinessStep>();
            var parallelStep = new ParallelStep("TestParallel", mockStep1.Object, mockStep2.Object, mockStep3.Object);

            // Act
            await parallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep2.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep3.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithSingleStep_StepExecuted()
        {
            // Arrange
            var mockStep = new Mock<IBusinessStep>();
            var parallelStep = new ParallelStep("SingleParallel", mockStep.Object);

            // Act
            await parallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithEmptySteps_CompletesSuccessfully()
        {
            // Arrange
            var parallelStep = new ParallelStep("EmptyParallel");

            // Act & Assert - should not throw
            Assert.DoesNotThrowAsync(async () =>
                await parallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }

        [Test]
        public void Name_ReturnsProvidedName()
        {
            // Arrange
            var expectedName = "MyParallelProcess";
            var parallelStep = new ParallelStep(expectedName);

            // Act & Assert
            Assert.That(parallelStep.Name, Is.EqualTo(expectedName));
        }

        [Test]
        [TestCase("ProcessA")]
        [TestCase("DataLoadingParallel")]
        [TestCase("ValidationBatch")]
        public void Name_WithVariousNames_ReturnsExactName(string name)
        {
            // Arrange
            var parallelStep = new ParallelStep(name);

            // Act & Assert
            Assert.That(parallelStep.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task ExecuteAsync_WithCancellationToken_PassesCancellationTokenToSteps()
        {
            // Arrange
            var mockStep = new Mock<IBusinessStep>();
            var parallelStep = new ParallelStep("CancellableParallel", mockStep.Object);
            var cancellationToken = new CancellationToken();

            // Act
            await parallelStep.ExecuteAsync(mockExecutionContext.Object, cancellationToken);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, cancellationToken), Times.Once);
        }

        [Test]
        public void ExecuteAsync_WhenStepThrows_ThrowsAggregateException()
        {
            // Arrange
            var mockStep1 = new Mock<IBusinessStep>();
            var mockStep2 = new Mock<IBusinessStep>();
            var testException = new InvalidOperationException("Test error");
            mockStep1.Setup(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(testException);

            var parallelStep = new ParallelStep("FailingParallel", mockStep1.Object, mockStep2.Object);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await parallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }
    }
}