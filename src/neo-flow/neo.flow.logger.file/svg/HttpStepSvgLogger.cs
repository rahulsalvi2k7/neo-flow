using neo.flow.core.Interfaces;

namespace neo.flow.logger.file.svg
{
    public class HttpStepSvgLogger : SvgLogger
    {
        public HttpStepSvgLogger(string svgPath) : base(svgPath) { }

        public override async Task LogExecutionAsync(string stepName, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            var x = context.Get<int>("x");
            var y = context.Get<int>("y");

            var ts = dateTimeProvider.UtcNow();

            // Rectangle for HTTP step
            var rect = $"<rect x='{x}' y='{y}' width='120' height='60' fill='white' stroke='black' stroke-width='2' />";

            // Text: step name and optional status
            var status = context.Get<int?>("lastHttpStatusCode");
            var statusText = status.HasValue ? $" ({status.Value})" : string.Empty;
            var text = $"<text x='{x + 10}' y='{y + 30}' font-family='Arial' font-size='12'>{System.Security.SecurityElement.Escape(stepName + statusText)}</text>";

            var svg = rect + text;

            await File.AppendAllTextAsync(_svgPath, Environment.NewLine + svg + Environment.NewLine);

            x += 140;

            await context.Set("x", x);
            await context.Set("y", y);
        }
    }
}
