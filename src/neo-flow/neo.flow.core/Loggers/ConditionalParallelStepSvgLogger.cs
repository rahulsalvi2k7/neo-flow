namespace neo.flow.core.Loggers
{
    public class ConditionalParallelStepSvgLogger : SvgLogger
    {
        public ConditionalParallelStepSvgLogger(string svgPath) : base(svgPath) { }

        public override async Task LogExecutionAsync(string stepName, DateTime startTime, DateTime endTime, Engine.ExecutionContext context)
        {
            // Diamond with O
            var diamond = "<polygon points='50,10 90,50 50,90 10,50' style='fill:white;stroke:black;stroke-width:2' />";
            var circle = "<circle cx='50' cy='50' r='20' style='stroke:black;stroke-width:2;fill:none' />";
            var svg = $"<svg width='100' height='100'>{diamond}{circle}</svg>";
            await File.AppendAllTextAsync(_svgPath, svg + Environment.NewLine);
        }
    }
}
