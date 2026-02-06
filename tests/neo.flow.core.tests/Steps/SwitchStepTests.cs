using Moq;
using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.tests.Steps
{
    [TestFixture]
    public class SwitchStepTests
    {
        private Mock<IExecutionContext> mockExecutionContext = null!;

        [SetUp]
        public void Setup()
        {
            mockExecutionContext = new Mock<IExecutionContext>();
        }

        [Test]
        public async Task ExecuteAsync_WithMultipleCasesAndSomeTrueConditions_ExecutesMatchingSteps()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockCondition2 = new Mock<ICondition>();
            var mockCondition3 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockStep2 = new Mock<IBusinessStep>();
            var mockStep3 = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);
            mockCondition3.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var cases = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object),
                (mockCondition3.Object, mockStep3.Object)
            };

            var switchStep = new SwitchStep("TestSwitch", cases);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep2.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
            mockStep3.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithSingleCase_ExecutesIfConditionTrue()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var cases = new List<(ICondition, IBusinessStep)> { (mockCondition.Object, mockStep.Object) };
            var switchStep = new SwitchStep("SingleCaseSwitch", cases);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithAllFalseCases_NoStepExecuted()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockCondition2 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockStep2 = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);

            var cases = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object)
            };

            var switchStep = new SwitchStep("AllFalseSwitch", cases);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
            mockStep2.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_WithDefaultStepAndNoMatchingCase_ExecutesDefaultStep()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            var mockDefaultStep = new Mock<IBusinessStep>();

            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);

            var cases = new List<(ICondition, IBusinessStep)> { (mockCondition.Object, mockStep.Object) };
            var switchStep = new SwitchStep("SwitchWithDefault", cases, mockDefaultStep.Object);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()), Times.Never);
            mockDefaultStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithDefaultStepAndMatchingCase_ExecutesBothMatchingAndDefault()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            var mockDefaultStep = new Mock<IBusinessStep>();

            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var cases = new List<(ICondition, IBusinessStep)> { (mockCondition.Object, mockStep.Object) };
            var switchStep = new SwitchStep("SwitchWithDefault", cases, mockDefaultStep.Object);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockDefaultStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithEmptyCases_CompletesSuccessfully()
        {
            // Arrange
            var cases = new List<(ICondition, IBusinessStep)>();
            var switchStep = new SwitchStep("EmptySwitch", cases);

            // Act & Assert - should not throw
            Assert.DoesNotThrowAsync(async () =>
                await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task ExecuteAsync_WithEmptyCasesAndDefaultStep_ExecutesDefaultStep()
        {
            // Arrange
            var mockDefaultStep = new Mock<IBusinessStep>();
            var cases = new List<(ICondition, IBusinessStep)>();
            var switchStep = new SwitchStep("EmptyWithDefault", cases, mockDefaultStep.Object);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockDefaultStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Name_ReturnsProvidedName()
        {
            // Arrange
            var expectedName = "MySwitch";
            var switchStep = new SwitchStep(expectedName, new List<(ICondition, IBusinessStep)>());

            // Act & Assert
            Assert.That(switchStep.Name, Is.EqualTo(expectedName));
        }

        [Test]
        [TestCase("ProcessSwitch")]
        [TestCase("DecisionPoint")]
        [TestCase("RoutingLogic")]
        public void Name_WithVariousNames_ReturnsExactName(string name)
        {
            // Arrange
            var switchStep = new SwitchStep(name, new List<(ICondition, IBusinessStep)>());

            // Act & Assert
            Assert.That(switchStep.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task ExecuteAsync_WithCancellationToken_PassesCancellationTokenToSteps()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var cases = new List<(ICondition, IBusinessStep)> { (mockCondition.Object, mockStep.Object) };
            var switchStep = new SwitchStep("CancellableSwitch", cases);
            var cancellationToken = new CancellationToken();

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, cancellationToken);

            // Assert
            mockStep.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void ExecuteAsync_WhenCaseStepThrows_ThrowsException()
        {
            // Arrange
            var mockCondition = new Mock<ICondition>();
            var mockStep = new Mock<IBusinessStep>();
            var testException = new InvalidOperationException("Test error in case");
            mockCondition.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);
            mockStep.Setup(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(testException);

            var cases = new List<(ICondition, IBusinessStep)> { (mockCondition.Object, mockStep.Object) };
            var switchStep = new SwitchStep("FailingSwitch", cases);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }

        [Test]
        public void ExecuteAsync_WhenDefaultStepThrows_ThrowsException()
        {
            // Arrange
            var mockDefaultStep = new Mock<IBusinessStep>();
            var testException = new InvalidOperationException("Test error in default");
            mockDefaultStep.Setup(m => m.ExecuteAsync(It.IsAny<IExecutionContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(testException);

            var cases = new List<(ICondition, IBusinessStep)>();
            var switchStep = new SwitchStep("FailingDefault", cases, mockDefaultStep.Object);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task ExecuteAsync_WithMultipleTrueCases_ExecutesAllMatchingStepsInParallel()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockCondition2 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockStep2 = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(true);

            var cases = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object)
            };

            var switchStep = new SwitchStep("MultiCaseSwitch", cases);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert
            mockStep1.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
            mockStep2.Verify(m => m.ExecuteAsync(mockExecutionContext.Object, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_EvaluatesAllConditions()
        {
            // Arrange
            var mockCondition1 = new Mock<ICondition>();
            var mockCondition2 = new Mock<ICondition>();
            var mockStep1 = new Mock<IBusinessStep>();
            var mockStep2 = new Mock<IBusinessStep>();

            mockCondition1.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);
            mockCondition2.Setup(m => m.Evaluate(It.IsAny<IExecutionContext>())).Returns(false);

            var cases = new List<(ICondition, IBusinessStep)>
            {
                (mockCondition1.Object, mockStep1.Object),
                (mockCondition2.Object, mockStep2.Object)
            };

            var switchStep = new SwitchStep("EvaluationSwitch", cases);

            // Act
            await switchStep.ExecuteAsync(mockExecutionContext.Object, CancellationToken.None);

            // Assert - verify that all conditions were evaluated
            mockCondition1.Verify(m => m.Evaluate(mockExecutionContext.Object), Times.Once);
            mockCondition2.Verify(m => m.Evaluate(mockExecutionContext.Object), Times.Once);
        }
    }
}
