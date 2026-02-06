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

        public WorkflowBuilder(string name)
        {
            _name = name;
        }

        public WorkflowBuilder Step(IBusinessStep step)
        {
            _steps.Add(step);
            return this;
        }

        public WorkflowBuilder Workflow(IWorkflow workflow)
        {
            _steps.Add(workflow);
            return this;
        }

        public IWorkflow Build()
        {
            return new BuiltWorkflow(_name, _steps.ToList());
        }
    }
}
