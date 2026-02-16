using neo.flow.core.Attributes;
using neo.flow.core.Decorators;
using neo.flow.core.Engine;
using neo.flow.core.Interfaces;
using System;
using System.Threading.Tasks;
using Jint;

namespace neo.flow.core.Steps
{
    /// <summary>
    /// Executes a JavaScript script with access to the ExecutionContext.
    /// </summary>
    public class ScriptStep : IBusinessStep
    {
        private readonly string _name;
        private readonly string _script;
        private readonly ILogger _logger;


        public ScriptStep(string name, string script, ILogger? logger = null)
        {
            _name = name;
            _script = script ?? throw new ArgumentNullException(nameof(script));
            _logger = logger ?? new TextLogger("workflow.log");
        }

        public string Name => _name;

        [LogExecution]
        public Task ExecuteAsync(IExecutionContext context, CancellationToken cancellationToken)
            => LoggingDecorator.InvokeWithLoggingAsync(ExecuteCoreAsync, context, cancellationToken, Name, _logger);

        private Task ExecuteCoreAsync(IExecutionContext context, CancellationToken cancellationToken)
        {
            if (context is not Engine.ExecutionContext execContext)
            {
                throw new ArgumentException("ScriptStep requires ExecutionContext implementation.", nameof(context));
            }

            var engine = new Jint.Engine(cfg => cfg.AllowClr());

            // Expose strongly-typed Get/Set wrappers for JS
            engine.SetValue("get", new Func<string, object?>(key => execContext.Get<object?>(key)));
            engine.SetValue("set", new Action<string, object?>((key, value) => execContext.Set(key, value).GetAwaiter().GetResult()));

            // Optionally expose the context object itself
            engine.SetValue("context", execContext);
            engine.Execute(_script);
            return Task.CompletedTask;
        }
    }
}
