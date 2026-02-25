using neo.flow.core.Interfaces;

namespace neo.flow.app
{
    internal class ContainsUnderscoreCondition : ICondition
    {
        public bool Evaluate(IExecutionContext context)
        {
            var name = context.Get<string>("name");

            if (name == null)
            {
                return false;
            }

            return name.Contains("_");
        }
    }
}
