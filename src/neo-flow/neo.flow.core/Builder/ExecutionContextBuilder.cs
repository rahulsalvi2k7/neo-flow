using neo.flow.core.Engine;
using neo.flow.core.Interfaces;
using neo.flow.core.Models;
using System.Collections.Generic;

namespace neo.flow.core.Builder
{
    /// <summary>
    /// Builder for ExecutionContext to set initial variables before workflow execution.
    /// </summary>
    public class ExecutionContextBuilder
    {
        private readonly Dictionary<string, object?> _initialData = new();
        private IDateTimeProvider? _dateTimeProvider;
        private string _actor = "Unknown";

        public ExecutionContextBuilder WithDateTimeProvider(IDateTimeProvider provider)
        {
            _dateTimeProvider = provider;
            return this;
        }

        public ExecutionContextBuilder WithActor(string actor)
        {
            _actor = actor;
            return this;
        }

        public ExecutionContextBuilder WithVariable<T>(string key, T value)
        {
            _initialData[key] = value;
            return this;
        }

        public Engine.ExecutionContext Build()
        {
            ArgumentNullException.ThrowIfNull(_dateTimeProvider);

            var context = new Engine.ExecutionContext(_dateTimeProvider);
            
            foreach (var kvp in _initialData)
            {
                // Set initial variables synchronously
                context.Set<object?>(kvp.Key, kvp.Value, _actor).GetAwaiter().GetResult();
            }

            return context;
        }
    }
}
