using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class LogBuilder
    {
        private string? _name;
        private ILogger? _logger;
        private IBusinessStep? _next;

        public LogBuilder(string name)
        {
            _name = name;
        }

        public LogBuilder Logger(ILogger? logger)
        {
            _logger = logger;
            return this;
        }

        public LogBuilder Next(IBusinessStep? next)
        {
            _next = next;
            return this;
        }

        public IBusinessStep Build()
        {
            return new LogStep(_name, _next, _logger);
        }
    }
}
