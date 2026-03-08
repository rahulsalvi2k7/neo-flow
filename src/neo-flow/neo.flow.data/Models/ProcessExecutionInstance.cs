namespace neo.flow.data.Models
{
    public sealed record ProcessExecutionInstance
    {
        public required string Id { get; set; }

        public required string ProcessId { get; set; }

        public required string Status { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
