using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.tests.Steps
{
    [TestFixture]
    public class ConditionalParallelStepTests
    {
        private Mock<IExecutionContext> mockExecutionContext = null!;

        [SetUp]
        public void Setup()
        {
            mockExecutionContext = new Mock<IExecutionContext>();
        }

        [Test]
        public async Task ExecuteAsync_WithMultipleTrueBranches_AllTrueStepsExecutedInParallel()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockCondition2 = new Mock<ICondition>();
            var mockStep2 = new Mock<IBusinessStep>();
            var mockCondition3 = new Mock<ICondition>();
            var mockStep3 = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);
            mockCondition3.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object),
                (mockCondition3.Object, mockStep3.Object),
            };

            var conditionalParallelStep = new ConditionalParallelStep("MultipleTrue", branches);

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep2.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep3.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_WithSingleTrueBranch_StepExecuted()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition.Object, mockStep.Object)
            };

            var conditionalParallelStep = new ConditionalParallelStep("SingleTrue", branches);

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithAllFalseBranches_NoStepsExecuted()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockCondition2 = new Mock<ICondition>();
            var mockStep2 = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object),
            };

            var conditionalParallelStep = new ConditionalParallelStep("AllFalse", branches);

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
            mockStep2.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_WithAllFalseBranchesAndDefaultStep_DefaultStepExecuted()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockCondition2 = new Mock<ICondition>();
            var mockStep2 = new Mock<IBusinessStep>();
            var mockDefaultStep = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object),
            };

            var conditionalParallelStep = new ConditionalParallelStep("WithDefault", branches, mockDefaultStep.Object);

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockDefaultStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep1.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
            mockStep2.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_WithTrueBranchesAndDefaultStep_OnlyTrueBranchesExecuted()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockDefaultStep = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
            };

            var conditionalParallelStep = new ConditionalParallelStep("WithDefault", branches, mockDefaultStep.Object);

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockDefaultStep.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_WithEmptyBranches_CompletesSuccessfully()
        {
            // Arrange
            var branches = new List<(ICondition, IBusinessStep)>();
            var conditionalParallelStep = new ConditionalParallelStep("Empty", branches);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
                await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task ExecuteAsync_WithEmptyBranchesAndDefaultStep_DefaultStepExecuted()
        {
            // Arrange
            var mockDefaultStep = new Mock<IBusinessStep>();
            var branches = new List<(ICondition, IBusinessStep)>();
            var conditionalParallelStep = new ConditionalParallelStep("EmptyWithDefault", branches, mockDefaultStep.Object);

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockDefaultStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Name_ReturnsProvidedName()
        {
            // Arrange
            var expectedName = "MyConditionalParallel";
            var branches = new List<(ICondition, IBusinessStep)>();
            var conditionalParallelStep = new ConditionalParallelStep(expectedName, branches);

            // Act & Assert
            Assert.That(conditionalParallelStep.Name, Is.EqualTo(expectedName));
        }

        [Test]
        [TestCase("ProcessA")]
        [TestCase("DataValidationParallel")]
        [TestCase("ConditionalBatchJob")]
        public void Name_WithVariousNames_ReturnsExactName(string name)
        {
            // Arrange
            var branches = new List<(ICondition, IBusinessStep)>();
            var conditionalParallelStep = new ConditionalParallelStep(name, branches);

            // Act & Assert
            Assert.That(conditionalParallelStep.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task ExecuteAsync_WithCancellationToken_PassesCancellationTokenToSteps()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition.Object, mockStep.Object)
            };

            var conditionalParallelStep = new ConditionalParallelStep("CancellableParallel", branches);
            var cancellationToken = new CancellationToken();

            // Act
            await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, cancellationToken);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, cancellationToken), Times.Once);
        }

        [Test]
        public void ExecuteAsync_WhenTrueBranchStepThrows_ThrowsException()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            var testException = new InvalidOperationException("Conditional parallel step error");

            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);
            mockStep.Setup(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(testException);

            var branches = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition.Object, mockStep.Object)
            };

            var conditionalParallelStep = new ConditionalParallelStep("FailingParallel", branches);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await conditionalParallelStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }
    }
}
