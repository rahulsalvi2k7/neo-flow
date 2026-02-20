using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class ConditionalBuilder
    {
        private readonly string _name;
        private readonly ICondition _condition;
        private readonly WorkflowBuilder _thenBuilder;
        private WorkflowBuilder? _elseBuilder;

        public ConditionalBuilder(string name, ICondition condition)
        {
            _name = name;
            _condition = condition;
            _thenBuilder = new WorkflowBuilder("Then");
        }

        public ConditionalBuilder Then(Action<WorkflowBuilder> then)
        {
            then(_thenBuilder);
            return this;
        }

        public ConditionalBuilder Else(Action<WorkflowBuilder> @else)
        {
            _elseBuilder = new WorkflowBuilder("Else");
            @else(_elseBuilder);
            return this;
        }

        public IBusinessStep Build()
        {
            return new ConditionalStep(
                _name,
                _condition,
                _thenBuilder.Build(),
                _elseBuilder?.Build()
            );
        }
    }
}
