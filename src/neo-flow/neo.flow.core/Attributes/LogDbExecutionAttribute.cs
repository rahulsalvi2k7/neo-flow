namespace neo.flow.core.Attributes
{
    /// <summary>
    /// Attribute to enable persiatance on ExecuteAsync methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LogDbExecutionAttribute : Attribute
    {
        public LogDbExecutionAttribute() { }
    }
}
