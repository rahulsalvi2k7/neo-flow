using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class ParallelBuilder
    {
        private readonly List<IBusinessStep> _branches = new();
        private string _name;
        private ILogger<ParallelStep>? _logger;

        public ParallelBuilder(string name)
        {
            _name = name;
        }

        public ParallelBuilder Logger(ILogger<ParallelStep>? logger)
        {
            _logger = logger;
            return this;
        }

        public ParallelBuilder Branch(IBusinessStep branch)
        {
            _branches.Add(branch);

            return this;
        }

        public IBusinessStep Build()
        {
            return new ParallelStep(_name, _logger, _branches.ToArray());
        }
    }
}
