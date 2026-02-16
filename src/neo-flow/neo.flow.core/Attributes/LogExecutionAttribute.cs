namespace neo.flow.core.Attributes
{
    /// <summary>
    /// Attribute to enable logging on ExecuteAsync methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LogExecutionAttribute : Attribute
    {
        public LogExecutionAttribute() { }
    }
}
