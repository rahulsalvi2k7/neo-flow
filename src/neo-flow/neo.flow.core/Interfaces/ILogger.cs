namespace neo.flow.core.Interfaces
{
    /// <summary>
    /// Logger interface for workflow steps.
    /// </summary>
    public interface ILogger
    {
        Task LogExecutionAsync(string stepName, IExecutionContext context);
    }

    /// <summary>
    ///  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILogger<T>
    {
        Task LogExecutionAsync(T t, IExecutionContext context);
    }

    /// <summary>
    /// IDbLogger
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbLogger<T> : ILogger<T> where T : IBusinessStep
    {
        Task LogStartExecutionAsync(T t, IExecutionContext context);

        Task LogEndExecutionAsync(T t, IExecutionContext context);
    }
}
