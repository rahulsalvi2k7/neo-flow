namespace neo.flow.core.Loggers
{
    public class ConditionalStepSvgLogger : SvgLogger
    {
        public ConditionalStepSvgLogger(string svgPath) : base(svgPath) { }

        public override async Task LogExecutionAsync(string stepName, DateTime startTime, DateTime endTime, Engine.ExecutionContext context)
        {
            // Diamond with X
            var diamond = "<polygon points='50,10 90,50 50,90 10,50' style='fill:white;stroke:black;stroke-width:2' />";
            var x = "<line x1='30' y1='30' x2='70' y2='70' style='stroke:black;stroke-width:2' />" +
                    "<line x1='70' y1='30' x2='30' y2='70' style='stroke:black;stroke-width:2' />";
            var svg = $"<svg width='100' height='100'>{diamond}{x}</svg>";
            await File.AppendAllTextAsync(_svgPath, svg + Environment.NewLine);
        }
    }
}
