using neo.flow.core.Builder;
using neo.flow.core.Builder.Extensions;
using neo.flow.core.logger.Console;
using neo.flow.core.Steps;

namespace neo.flow.app
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var startStepLogger = new StartStepConsoleLogger();
            var endStepLogger = new EndStepConsoleLogger();

            var workflow = new WorkflowBuilder("sample-workflow")
            .Step(new StartStep("start1", startStepLogger))
            .Parallel("check names", branches =>
            {
            branches
                .Branch(
                    new WorkflowBuilder("z")
                    .Parallel(
                        ".1",
                        branches =>
                        {
                            branches
                                .Branch(new LogStep("1.1"))
                                .Branch(new LogStep("1.2"));
                        })
                    .Build())
                .Branch(new LogStep("2"));
            })
            .Step(new EndStep("end1", endStepLogger))
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
