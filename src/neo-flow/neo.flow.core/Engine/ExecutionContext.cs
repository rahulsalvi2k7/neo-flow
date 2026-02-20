using neo.flow.core.Interfaces;
using neo.flow.core.Models;
using System.Collections.Concurrent;

namespace neo.flow.core.Engine
{
    public sealed class ExecutionContext : IExecutionContext
    {
        private readonly ConcurrentDictionary<string, object?> _data = new();
        private readonly ConcurrentStack<AuditEntry> _audit = new();
        private readonly IDateTimeProvider _dateTimeProvider;

        public ExecutionContext(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public IDateTimeProvider DateTimeProvider => _dateTimeProvider;

        public T? Get<T>(string key)
            => _data.TryGetValue(key, out var value) ? (T?)value : default;

        public Task Set<T>(string key, T value, string actor = "Unknown")
        {
            _data.AddOrUpdate(
                key,
                _ =>
                {
                    _audit.Push(new AuditEntry(
                        Timestamp: _dateTimeProvider.UtcNow(),
                        Key: key,
                        OldValue: null,
                        NewValue: value,
                        Actor: actor));
                    return value;
                },
                (_, old) =>
                {
                    _audit.Push(new AuditEntry(
                        Timestamp: _dateTimeProvider.UtcNow(),
                        Key: key,
                        OldValue: old,
                        NewValue: value,
                        Actor: actor));
                    return value;
                });

            return Task.CompletedTask;
        }

        public Task<List<AuditEntry>> GetAuditTrail()
        {
            return Task.FromResult(_audit.ToList());
        }
    }
}
