using neo.flow.core.Base;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Builder
{
    public sealed class BuiltWorkflow : WorkflowBase
    {
        private readonly IReadOnlyList<IBusinessStep> _steps;

        public BuiltWorkflow(string name, IReadOnlyList<IBusinessStep> steps)
        {
            Name = name;
            _steps = steps;
        }

        public override string Name { get; }

        protected override IReadOnlyList<IBusinessStep> Steps => _steps;
    }
}
