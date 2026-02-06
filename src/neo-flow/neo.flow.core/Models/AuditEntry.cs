namespace neo.flow.core.Models
{
    /// <summary>
    /// Represents an audit entry that records a change made during workflow execution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This record captures the details of a specific change, including when it occurred,
    /// what was changed, the old and new values, and which workflow step or component
    /// initiated the change. Audit entries are used for tracking and auditing purposes
    /// in workflow systems.
    /// </para>
    /// </remarks>
    public sealed record AuditEntry(
        /// <summary>
        /// Gets the date and time when the audit entry was created.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the moment the change was recorded.</value>
        DateTime Timestamp,

        /// <summary>
        /// Gets the key or property name that was changed.
        /// </summary>
        /// <value>A <see langword="string"/> containing the name of the property or field that was modified.</value>
        string Key,

        /// <summary>
        /// Gets the value before the change was applied.
        /// </summary>
        /// <value>
        /// An <see langword="object"/> containing the previous value, or <see langword="null"/> if the value was
        /// not available or the property is newly created.
        /// </value>
        object? OldValue,

        /// <summary>
        /// Gets the value after the change was applied.
        /// </summary>
        /// <value>
        /// An <see langword="object"/> containing the new value, or <see langword="null"/> if the value was
        /// set to <see langword="null"/> or the property was deleted.
        /// </value>
        object? NewValue,

        /// <summary>
        /// Gets the name of the step or workflow component that initiated the change.
        /// </summary>
        /// <value>A <see langword="string"/> identifying the actor (step name or workflow name) responsible for the change.</value>
        string Actor
    );
}
