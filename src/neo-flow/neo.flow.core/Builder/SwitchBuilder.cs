using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class SwitchBuilder
    {
        private readonly List<(ICondition, IBusinessStep)> _cases = new();
        private IBusinessStep? _default;

        public SwitchBuilder Case(ICondition condition, Action<WorkflowBuilder> build)
        {
            var wb = new WorkflowBuilder("Case");
            build(wb);
            _cases.Add((condition, wb.Build()));
            return this;
        }

        public SwitchBuilder Default(Action<WorkflowBuilder> build)
        {
            var wb = new WorkflowBuilder("Default");
            build(wb);
            _default = wb.Build();
            return this;
        }

        public IBusinessStep Build(string name)
        {
            return new SwitchStep(name, _cases, _default);
        }
    }
}
