using neo.flow.core.Steps;
using neo.flow.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neo.flow.core.Builder
{
    public sealed class WorkflowBuilder
    {
        private readonly string _name;
        private readonly List<IBusinessStep> _steps = new();
        private ExecutionContextBuilder? _contextBuilder;

        public WorkflowBuilder(string name)
        {
            _name = name;
        }

        public WorkflowBuilder Step(IBusinessStep step)
        {
            _steps.Add(step);
            return this;
        }

        /// <summary>
        /// Adds a JavaScript script step to the workflow.
        /// </summary>
        /// <param name="script">The JavaScript code to execute. Has access to get(key) and set(key, value).</param>
        public WorkflowBuilder StepScript(string script)
        {
            _steps.Add(new ScriptStep(string.Empty, script));
            return this;
        }

        public WorkflowBuilder Workflow(IWorkflow workflow)
        {
            _steps.Add(workflow);
            return this;
        }

        /// <summary>
        /// Sets the initial ExecutionContextBuilder for context variables.
        /// </summary>
        public WorkflowBuilder WithExecutionContextBuilder(ExecutionContextBuilder builder)
        {
            _contextBuilder = builder;
            return this;
        }

        public IWorkflow Build()
        {
            // Optionally pass contextBuilder to BuiltWorkflow if needed
            return new BuiltWorkflow(_name, _steps.ToList());
        }
    }
}
