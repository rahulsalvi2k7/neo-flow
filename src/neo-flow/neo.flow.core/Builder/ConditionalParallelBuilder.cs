using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class ConditionalParallelBuilder
    {
        private readonly List<(ICondition, IBusinessStep)> _branches = new();
        private IBusinessStep? _default;

        public ConditionalParallelBuilder When(
            ICondition condition,
            Action<WorkflowBuilder> build)
        {
            var wb = new WorkflowBuilder("ConditionalParallelBranch");
            build(wb);
            _branches.Add((condition, wb.Build()));
            return this;
        }

        public ConditionalParallelBuilder Default(Action<WorkflowBuilder> build)
        {
            var wb = new WorkflowBuilder("ConditionalParallelDefault");
            build(wb);
            _default = wb.Build();
            return this;
        }

        public IBusinessStep Build(string name)
        {
            return new ConditionalParallelStep(name, _branches, _default);
        }
    }
}
