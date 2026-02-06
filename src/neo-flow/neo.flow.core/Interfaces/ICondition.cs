namespace neo.flow.core.Interfaces
{
    public interface ICondition
    {
        bool Evaluate(IExecutionContext context);
    }
}
