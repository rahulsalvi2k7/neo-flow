using neo.flow.core.Models;

namespace neo.flow.core.Interfaces
{
    public interface IExecutionContext
    {
        T? Get<T>(string key);

        Task Set<T>(string key, T value, string actor = "Unknown");

        Task<List<AuditEntry>> GetAuditTrail();
    }
}
