using neo.flow.core.Base;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Builder
{
    public sealed class Workflow : WorkflowBase
    {
        public readonly IReadOnlyList<IBusinessStep> _steps;

        public Workflow(string name, IReadOnlyList<IBusinessStep> steps)
        {
            Name = name;
            _steps = steps;
        }

        public override string Name { get; }

        public override IReadOnlyList<IBusinessStep> Steps => _steps;
    }
}
