using neo.flow.core.Interfaces;

namespace neo.flow.app
{
    internal class ContainsSpaceCondition : ICondition
    {
        public bool Evaluate(IExecutionContext context)
        {
            var name = context.Get<string>("name");

            if (name is null) 
            {
                return false;
            }

            return name.Contains(" ");
        }
    }
}
