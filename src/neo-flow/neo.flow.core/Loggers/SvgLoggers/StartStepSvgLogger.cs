namespace neo.flow.core.Loggers.SvgLoggers
{
    public class StartStepSvgLogger : SvgLogger
    {
        public StartStepSvgLogger(string svgPath) : base(svgPath) { }

        public override async Task LogExecutionAsync(string stepName, DateTime startTime, DateTime endTime, Engine.ExecutionContext context)
        {
            var x = context.Get<int>("x");
            var y = context.Get<int>("y");

            // Circle with right-pointing triangle
            var circle = $"<circle cx='{x+50}' cy='{y+50}' r='40' fill='white' stroke='black' stroke-width='2' />";
            
            // Triangle points (right-pointing, centered)
            var triangle = $"<polygon points='{x+40} {y+15},{x+70} {y+55},{x+40} {y+85},{x+40} {y}' fill='black' />";
            
            var svg = $"{circle}{triangle}";
            
            await File.AppendAllTextAsync(_svgPath, Environment.NewLine + svg + Environment.NewLine);

            x += 100;

            await context.Set("x", x);
            await context.Set("y", y);
        }
    }
}
