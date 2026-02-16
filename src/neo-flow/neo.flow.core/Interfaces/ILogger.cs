namespace neo.flow.core.Interfaces
{
    /// <summary>
    /// Logger interface for workflow steps.
    /// </summary>
    public interface ILogger
    {
        Task LogExecutionAsync(string stepName, DateTime startTime, DateTime endTime, Engine.ExecutionContext context);
    }
}
