using neo.flow.core.Builder;
using neo.flow.core.Builder.Extensions;
using neo.flow.core.logger.Console;
using neo.flow.core.Steps;

namespace neo.flow.app
{
    internal class Program
    {
        private static readonly StartStepConsoleLogger startStepLogger = new();
        private static readonly EndStepConsoleLogger endStepLogger = new();

        private static readonly StartStep startStep = new("start1", startStepLogger);
        private static readonly EndStep endStep1 = new("end1", endStepLogger);

        private static readonly LogStep logSteps_1 = new("1");
        private static readonly LogStep logSteps_1_1 = new("1_1");
        private static readonly LogStep logSteps_1_2 = new("1_2");
        private static readonly LogStep logSteps_2 = new("2");
        private static readonly LogStep logSteps_3 = new("3");

        private static readonly Action<ConditionalParallelBuilder> checkNames = branches => branches
                .When(
                    condition: new ContainsSpaceCondition(),
                    build: b => new WorkflowBuilder("a")
                        .Step(new LogStep("a.1"))
                        .Build())
                .When(
                    condition: new ContainsUnderscoreCondition(),
                    build: b => new WorkflowBuilder("b")
                        .Step(new LogStep("b.1"))
                        .Build())
                .Default(
                    build: b => new WorkflowBuilder("c")
                        .Step(new LogStep("c.1"))
                        .Build()
                );

        private static readonly Action<ParallelBuilder> logNames = branches => branches
                .Branch(
                    new WorkflowBuilder("y")
                    .Parallel(
                        name: ".1",
                        branches =>
                        {
                            branches
                                .Branch(logSteps_1_1)
                                .Branch(logSteps_1_2);
                        })
                    .Build())
                .Branch(logSteps_2);

        private static readonly Action<SequentialBuilder> logNamesSeq = branches =>
        {
            branches
                .Branch(
                    new WorkflowBuilder("z")
                    .Parallel(
                        name: ".2",
                        branches =>
                        {
                            branches
                                .Branch(logSteps_1_1)
                                .Branch(logSteps_1_2);
                        })
                    .Build())
                .Branch(logSteps_3);
        };

        static async Task Main(string[] args)
        {
            // Build the workflow
            var workflow = new WorkflowBuilder("sample-workflow")
                .Step(startStep)
                .Step(logSteps_1)
                .Parallel("log names", logNames)
                .ConditionalParallel("check if names", checkNames)
                .Sequential("log names seq", logNamesSeq)
                .Step(logSteps_3)
                .Step(endStep1)
            .Build();

            // Build dependencies 
            var dateTimeProvider = new DateTimeProvider();

            // Build execution context
            var executionContextBuilder = new ExecutionContextBuilder();

            var executionContext = executionContextBuilder
                .WithDateTimeProvider(dateTimeProvider)
                .WithVariable("id", Guid.NewGuid())
                .WithVariable("names", new[] { "John Doe", "Jane Doe" })
                .Build();

            // Execute the workflow
            await workflow.ExecuteAsync(executionContext, CancellationToken.None);
        }
    }
}
