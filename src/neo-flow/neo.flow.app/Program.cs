using neo.flow.core.Builder;

namespace neo.flow.app
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var workflowBuilder = new WorkflowBuilder("sample-workflow");

            var workflow = workflowBuilder
                .Step( 
                    
                )
                .Step(new ParallelBuilder("Check Name")
                    .Branch(_logStepBuilder("1"))
                    .Branch(_logStepBuilder("2"))
                    .Build())
                .Step(
                    new EndBuilder()
                        .Name("end1")
                        .Build())
                .Build();

            var dateTimeProvider = new DateTimeProvider();
            var executionContextBuilder = new ExecutionContextBuilder();
            var executionContext = executionContextBuilder
                .WithDateTimeProvider(dateTimeProvider)
                .WithVariable("id", 1)
                .Build();

            await workflow.ExecuteAsync(executionContext, CancellationToken.None);
        }

        private static Func<string, Action<WorkflowBuilder>> _startStepBuilder = (s) =>
        {
            return (b) => b.Step(new StartBuilder(s).Build());
        };

        private static Func<string, Action<WorkflowBuilder>> _logStepBuilder = (s) =>
        {
            return (b) => b.Step(new LogBuilder(s).Build());
        };
    }
}

