using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class StartBuilder
    {
        private string? _name;
        private ILogger<IBusinessStep>? _logger;

        public StartBuilder(string name)
        {
            _name = name;
        }

        public StartBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public StartBuilder Logger(ILogger<IBusinessStep>? logger)
        {
            _logger = logger;
            return this;
        }

        public IBusinessStep Build()
        {
            return new StartStep(_name ?? "End");
        }
    }
}
