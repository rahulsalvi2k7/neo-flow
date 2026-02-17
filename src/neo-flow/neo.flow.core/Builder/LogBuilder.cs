using neo.flow.core.Interfaces;
using neo.flow.core.Steps;

namespace neo.flow.core.Builder
{
    public sealed class LogBuilder
    {
        private string? _name;
        private ILogger<LogStep>? _logger;

        public LogBuilder(string name)
        {
            _name = name;
        }

        public LogBuilder Logger(ILogger<LogStep>? logger)
        {
            _logger = logger;
            return this;
        }

        public IBusinessStep Build()
        {
            return new LogStep(_name, _logger);
        }
    }
}
