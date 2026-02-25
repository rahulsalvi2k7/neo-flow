using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class SequentialBuilder
    {
        private readonly List<IBusinessStep> _branches = new();
        private string _name;
        private ILogger<SequentialStep>? _logger;

        public SequentialBuilder(string name)
        {
            _name = name;
        }

        public SequentialBuilder Logger(ILogger<SequentialStep>? logger)
        {
            _logger = logger;
            return this;
        }

        public SequentialBuilder Branch(IBusinessStep branch)
        {
            _branches.Add(branch);

            return this;
        }

        public IBusinessStep Build()
        {
            return new SequentialStep(_name, _logger, _branches.ToArray());
        }
    }
}
