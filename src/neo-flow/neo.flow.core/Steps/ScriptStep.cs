using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Interfaces;

namespace neo.flow.core.Steps
{
    /// <summary>
    /// Executes a JavaScript script with access to the ExecutionContext.
    /// </summary>
    public class ScriptStep : IBusinessStep
    {
        private readonly string _name;
        private readonly string _script;
        private readonly ILogger<ScriptStep>? _logger;

        public ScriptStep(string name, string script, ILogger<ScriptStep>? logger = null)
        {
            _name = name;
            _script = script ?? throw new ArgumentNullException(nameof(script));
            _logger = logger;
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken cancellationToken)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, cancellationToken, this, _logger);

        private async Task ExecuteCoreAsync(IExecutionContext context, CancellationToken cancellationToken)
        {
            try
            {
                if (context is not Engine.ExecutionContext execContext)
                {
                    throw new ArgumentException("ScriptStep requires ExecutionContext implementation.", nameof(context));
                }

                var engine = new Jint.Engine();

                // Expose strongly-typed Get/Set wrappers for JS
                engine.SetValue("get", new Func<string, object?>(key => execContext.Get<object?>(key)));
                engine.SetValue("set", new Action<string, object?>((key, value) => execContext.Set(key, value).GetAwaiter().GetResult()));

                // Optionally expose the context object itself
                engine.SetValue("context", execContext);
                engine.Execute(_script);
            }
            catch (Exception ex)
            {
                await context.Set("ScriptError", ex.Message);

                await _logger?.LogExecutionAsync(this, context.DateTimeProvider, context);

                throw;
            }
        }
    }
}
