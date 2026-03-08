using neo.flow.core.Interfaces;

namespace neo.flow.data.Models
{
    public sealed class BusinessStepExecutionInstance
    {
        public required string Id { get; set; }

        public string? ProcessExecutionInstanceId { get; set; }

        public string? BusinessStepId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }

    public sealed record BusinessStepExecutionInstance<T> where T : IBusinessStep
    {
        public required string Id { get; set; }

        public required string ProcessExecutionInstanceId { get; set; }

        public required T BusinessStep { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
