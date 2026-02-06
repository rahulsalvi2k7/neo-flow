using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class ParallelBuilder
    {
        private readonly List<IBusinessStep> _branches = new();

        public ParallelBuilder Branch(Action<WorkflowBuilder> build)
        {
            var wb = new WorkflowBuilder("ParallelBranch");
            build(wb);
            _branches.Add(wb.Build());
            return this;
        }

        public IBusinessStep Build(string name)
        {
            return new ParallelStep(name, _branches.ToArray());
        }
    }
}
