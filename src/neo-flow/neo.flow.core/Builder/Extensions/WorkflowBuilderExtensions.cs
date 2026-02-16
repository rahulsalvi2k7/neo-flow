using neo.flow.core.Interfaces;

namespace neo.flow.core.Builder.Extensions
{
    public static class WorkflowBuilderExtensions
    {
        public static WorkflowBuilder If(this WorkflowBuilder builder,
            ICondition condition,
            Action<WorkflowBuilder> then,
            Action<WorkflowBuilder>? @else = null)
        {
            var conditional = new ConditionalBuilder(condition)
                .Then(then);

            if (@else != null)
            {
                conditional.Else(@else);
            }

            builder.Step(conditional.Build());
            return builder;
        }

        public static WorkflowBuilder Switch(this WorkflowBuilder builder,
            string name,
            Action<SwitchBuilder> configure)
        {
            var sb = new SwitchBuilder();
            configure(sb);
            builder.Step(sb.Build(name));
            return builder;
        }

        public static WorkflowBuilder Parallel(this WorkflowBuilder builder,
            string name,
            Action<ParallelBuilder> configure)
        {
            var pb = new ParallelBuilder();
            configure(pb);
            builder.Step(pb.Build());
            return builder;
        }

        public static WorkflowBuilder ConditionalParallel(this WorkflowBuilder builder,
            string name,
            Action<ConditionalParallelBuilder> configure)
        {
            var cpb = new ConditionalParallelBuilder();
            configure(cpb);
            builder.Step(cpb.Build(name));
            return builder;
        }
    }
}
