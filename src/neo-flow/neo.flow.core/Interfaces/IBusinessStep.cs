namespace neo.flow.core.Interfaces
{
    public interface IBusinessStep
    {
        string Name { get; }

        Task ExecuteAsync(IExecutionContext context, CancellationToken ct);
    }
}
