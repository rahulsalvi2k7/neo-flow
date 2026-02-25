using neo.flow.core.Builder;
using neo.flow.core.Builder.Extensions;
using neo.flow.core.logger.Console;
using neo.flow.core.Steps;

namespace neo.flow.app
{
    internal class Program
    {
        private static StartStepConsoleLogger startStepLogger = new StartStepConsoleLogger();
        private static EndStepConsoleLogger endStepLogger = new EndStepConsoleLogger();

        private static StartStep startStep = new StartStep("start1", startStepLogger);
        private static EndStep endStep1 = new EndStep("end1", endStepLogger);

        private static LogStep logSteps_1_1 = new LogStep("1_1");
        private static LogStep logSteps_1_2 = new LogStep("1_2");
        
        private static Action<ConditionalParallelBuilder> checkNames = branches =>
        {
            branches
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
        };

        private static Action<ParallelBuilder> logNames = branches =>
        {
            branches
                .Branch(
                    new WorkflowBuilder("z")
                    .Parallel(
                        name: ".1",
                        branches =>
                        {
                            branches
                                .Branch(logSteps_1_1)
                                .Branch(logSteps_1_2);
                        })
                    .Build())
                .Branch(new LogStep("2"));
        };

        static async Task Main(string[] args)
        {            
            var workflow = new WorkflowBuilder("sample-workflow")
                .Step(startStep)
                .Parallel("log names", logNames)
                .ConditionalParallel("check if names", checkNames)
                .Step(endStep1)
            .Build();

            var dateTimeProvider = new DateTimeProvider();
            var executionContextBuilder = new ExecutionContextBuilder();
            var executionContext = executionContextBuilder
                .WithDateTimeProvider(dateTimeProvider)
                .WithVariable("id", Guid.NewGuid())
                .Build();

            await workflow.ExecuteAsync(executionContext, CancellationToken.None);
        }
    }
}
