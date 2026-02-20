using neo.flow.core.Interfaces;

namespace neo.flow.logger.file.svg
{
    public class EndStepSvgLogger : SvgLogger
    {
        public EndStepSvgLogger(string svgPath) : base(svgPath) { }

        public override async Task LogExecutionAsync(string stepName, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            var x = context.Get<int>("x");
            var y = context.Get<int>("y");

            // Circle with square inside
            var circle = $"<circle cx='{x + 50}' cy='{y + 50}' r='40' fill='white' stroke='black' stroke-width='2' />";

            // Square centered in the circle
            var square = $"<rect x='{x + 30}' y='{y + 30}' width='40' height='40' fill='black' />";

            var svg = $"{circle}{square}";

            await File.AppendAllTextAsync(_svgPath, Environment.NewLine + svg + Environment.NewLine);
        }
    }
}
