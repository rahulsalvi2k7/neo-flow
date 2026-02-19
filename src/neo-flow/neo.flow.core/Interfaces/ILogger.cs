namespace neo.flow.core.Interfaces
{
    /// <summary>
    /// Logger interface for workflow steps.
    /// </summary>
    public interface ILogger
    {
        Task LogExecutionAsync(string stepName, IDateTimeProvider dateTimeProvider, IExecutionContext context);
    }

    /// <summary>
    ///  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILogger<T>
    {
        Task LogExecutionAsync(T t, IDateTimeProvider dateTimeProvider, IExecutionContext context);
    }
}
