using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class EndBuilder
    {
        private string? _name;
        private ILogger<EndStep>? _logger;

        public EndBuilder(string name)
        {
            _name = name;
        }

        public EndBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public EndBuilder Logger(ILogger<EndStep>? logger)
        {
            _logger = logger;
            return this;
        }

        public IBusinessStep Build()
        {
            return new EndStep(_name ?? "Stop", _logger);
        }
    }
}
