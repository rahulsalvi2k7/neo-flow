using Moq;
using neo.flow.core.Builder;
using neo.flow.core.Interfaces;
using NUnit.Framework;

namespace neo.flow.core.tests.Builder
{
    [TestFixture]
    public class WorkflowBuilderTests
    {
        private Mock<IBusinessStep> mockBusinessStep = null!;
        private Mock<IWorkflow> mockWorkflow = null!;

        [SetUp]
        public void Setup()
        {
            mockBusinessStep = new Mock<IBusinessStep>();
            mockWorkflow = new Mock<IWorkflow>();
        }

        [Test]
        public void Constructor_WithName_InitializesBuilder()
        {
            // Arrange
            var name = "TestWorkflow";

            // Act
            var builder = new WorkflowBuilder(name);

            // Assert
            Assert.That(builder, Is.Not.Null);
        }

        [Test]
        public void Step_AddsSingleStep_ReturnsBuilderForChaining()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var step = mockBusinessStep.Object;

            // Act
            var result = builder.Step(step);

            // Assert
            Assert.That(result, Is.SameAs(builder));
        }

        [Test]
        public void Step_AddsSingleStep_IncludesStepInBuiltWorkflow()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var step = mockBusinessStep.Object;

            // Act
            builder.Step(step);
            var workflow = builder.Build();

            // Assert
            Assert.That(workflow, Is.Not.Null);
            Assert.That(workflow, Is.InstanceOf<BuiltWorkflow>());
        }

        [Test]
        public void Step_AddMultipleSteps_PreservesOrder()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var step1 = new Mock<IBusinessStep>().Object;
            var step2 = new Mock<IBusinessStep>().Object;
            var step3 = new Mock<IBusinessStep>().Object;

            // Act
            builder.Step(step1).Step(step2).Step(step3);
            var workflow = (BuiltWorkflow)builder.Build();

            // Assert
            Assert.That(workflow.Steps.Count, Is.EqualTo(3));
            Assert.That(workflow.Steps[0], Is.SameAs(step1));
            Assert.That(workflow.Steps[1], Is.SameAs(step2));
            Assert.That(workflow.Steps[2], Is.SameAs(step3));
        }

        [Test]
        public void Workflow_AddsSingleWorkflow_ReturnsBuilderForChaining()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var workflow = mockWorkflow.Object;

            // Act
            var result = builder.Workflow(workflow);

            // Assert
            Assert.That(result, Is.SameAs(builder));
        }

        [Test]
        public void Workflow_AddsSingleWorkflow_IncludesWorkflowInBuiltWorkflow()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var workflow = mockWorkflow.Object;

            // Act
            builder.Workflow(workflow);
            var builtWorkflow = builder.Build();

            // Assert
            Assert.That(builtWorkflow, Is.Not.Null);
        }

        [Test]
        public void Workflow_AddMultipleWorkflows_PreservesOrder()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var workflow1 = new Mock<IWorkflow>().Object;
            var workflow2 = new Mock<IWorkflow>().Object;
            var workflow3 = new Mock<IWorkflow>().Object;

            // Act
            builder.Workflow(workflow1).Workflow(workflow2).Workflow(workflow3);
            var builtWorkflow = (BuiltWorkflow)builder.Build();

            // Assert
            Assert.That(builtWorkflow.Steps.Count, Is.EqualTo(3));
            Assert.That(builtWorkflow.Steps[0], Is.SameAs(workflow1));
            Assert.That(builtWorkflow.Steps[1], Is.SameAs(workflow2));
            Assert.That(builtWorkflow.Steps[2], Is.SameAs(workflow3));
        }

        [Test]
        public void Step_And_Workflow_Mixed_PreservesOrder()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var step1 = new Mock<IBusinessStep>().Object;
            var workflow1 = new Mock<IWorkflow>().Object;
            var step2 = new Mock<IBusinessStep>().Object;
            var workflow2 = new Mock<IWorkflow>().Object;

            // Act
            builder.Step(step1).Workflow(workflow1).Step(step2).Workflow(workflow2);
            var builtWorkflow = (BuiltWorkflow)builder.Build();

            // Assert
            Assert.That(builtWorkflow.Steps.Count, Is.EqualTo(4));
            Assert.That(builtWorkflow.Steps[0], Is.SameAs(step1));
            Assert.That(builtWorkflow.Steps[1], Is.SameAs(workflow1));
            Assert.That(builtWorkflow.Steps[2], Is.SameAs(step2));
            Assert.That(builtWorkflow.Steps[3], Is.SameAs(workflow2));
        }

        [Test]
        public void Build_EmptyBuilder_CreatesWorkflowWithNoSteps()
        {
            // Arrange
            var builder = new WorkflowBuilder("EmptyWorkflow");

            // Act
            var workflow = (BuiltWorkflow)builder.Build();

            // Assert
            Assert.That(workflow.Steps.Count, Is.EqualTo(0));
        }

        [Test]
        public void Build_ReturnsWorkflowWithCorrectName()
        {
            // Arrange
            var workflowName = "MyWorkflow";
            var builder = new WorkflowBuilder(workflowName);

            // Act
            var workflow = builder.Build();

            // Assert
            Assert.That(workflow.Name, Is.EqualTo(workflowName));
        }

        [Test]
        public void Build_WithStepsAndWorkflows_ReturnsBuiltWorkflowInterface()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            builder.Step(mockBusinessStep.Object).Workflow(mockWorkflow.Object);

            // Act
            var workflow = builder.Build();

            // Assert
            Assert.That(workflow, Is.InstanceOf<IWorkflow>());
            Assert.That(workflow, Is.InstanceOf<IBusinessStep>());
        }

        [Test]
        public void Build_MultipleTimes_CreatesIndependentInstances()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            builder.Step(mockBusinessStep.Object);

            // Act
            var workflow1 = builder.Build();
            var workflow2 = builder.Build();

            // Assert
            Assert.That(workflow1, Is.Not.SameAs(workflow2));
        }

        [Test]
        [TestCase("Workflow1")]
        [TestCase("Workflow2")]
        [TestCase("Complex-Workflow_Name")]
        public void Build_WithDifferentNames_PreservesNameCorrectly(string workflowName)
        {
            // Arrange
            var builder = new WorkflowBuilder(workflowName);

            // Act
            var workflow = builder.Build();

            // Assert
            Assert.That(workflow.Name, Is.EqualTo(workflowName));
        }

        [Test]
        public void Chaining_MultipleOperations_AllMethodsReturnBuilder()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");

            // Act & Assert - Fluent chaining should work without errors
            var result = builder
                .Step(new Mock<IBusinessStep>().Object)
                .Step(new Mock<IBusinessStep>().Object)
                .Workflow(new Mock<IWorkflow>().Object)
                .Step(new Mock<IBusinessStep>().Object);

            Assert.That(result, Is.SameAs(builder));
        }

        [Test]
        public void Step_SameStepAddedMultipleTimes_BothIncludedInWorkflow()
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");
            var step = new Mock<IBusinessStep>().Object;

            // Act
            builder.Step(step).Step(step).Step(step);
            var workflow = (BuiltWorkflow)builder.Build();

            // Assert
            Assert.That(workflow.Steps.Count, Is.EqualTo(3));
            Assert.That(workflow.Steps[0], Is.SameAs(step));
            Assert.That(workflow.Steps[1], Is.SameAs(step));
            Assert.That(workflow.Steps[2], Is.SameAs(step));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void Step_AddingMultipleSteps_CountIsCorrect(int stepCount)
        {
            // Arrange
            var builder = new WorkflowBuilder("TestWorkflow");

            // Act
            for (int i = 0; i < stepCount; i++)
            {
                builder.Step(new Mock<IBusinessStep>().Object);
            }
            var workflow = (BuiltWorkflow)builder.Build();

            // Assert
            Assert.That(workflow.Steps.Count, Is.EqualTo(stepCount));
        }
    }
}