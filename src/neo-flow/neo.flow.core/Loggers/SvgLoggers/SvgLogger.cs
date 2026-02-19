using neo.flow.core.Interfaces;

namespace neo.flow.core.Loggers.SvgLoggers
{
    public abstract class SvgLogger : ILogger
    {
        protected readonly string _svgPath;
        protected SvgLogger(string svgPath)
        {
            _svgPath = svgPath;
        }

        public abstract Task LogExecutionAsync(string stepName, IDateTimeProvider dateTimeProvider, IExecutionContext context);
    }
}
