using neo.flow.core.Interfaces;

namespace neo.flow.logger.file.svg
{
    public class SequentialSvgLogger : SvgLogger
    {
        public SequentialSvgLogger(string svgPath) : base(svgPath) { }

        public override async Task LogExecutionAsync(string stepName, IDateTimeProvider dateTimeProvider, IExecutionContext context)
        {
            // Diamond with +
            var diamond = "<polygon points='50,10 90,50 50,90 10,50' style='fill:white;stroke:black;stroke-width:2' />";
            var plus = "<line x1='50' y1='30' x2='50' y2='70' style='stroke:black;stroke-width:2' />" +
                        "<line x1='30' y1='50' x2='70' y2='50' style='stroke:black;stroke-width:2' />";
            var svg = $"<svg width='100' height='100'>{diamond}{plus}</svg>";
            await File.AppendAllTextAsync(_svgPath, svg + Environment.NewLine);
        }
    }
}
