using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class StartBuilder
    {
        private string? _name;
        private IBusinessStep? _next;
        private ILogger? _logger;

        public StartBuilder(string name)
        {
            _name = name;
        }

        public StartBuilder Logger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public StartBuilder Next(IBusinessStep next)
        {
            _next = next;
            return this;
        }

        public IBusinessStep Build()
        {
            return new StartStep(_name ?? "Start", _next, _logger);
        }
    }
}
