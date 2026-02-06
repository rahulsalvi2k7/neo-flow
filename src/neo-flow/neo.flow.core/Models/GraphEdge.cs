namespace neo.flow.core.Models
{
    public sealed record GraphEdge(
        string From,
        string To,
        string? Label);
}
