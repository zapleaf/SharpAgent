namespace SharpAgent.Domain.Enums;

/// <summary>
/// Represents the possible states of a multi-agent workflow
/// </summary>
public enum WorkflowStatus
{
    NotStarted,
    Running,
    Completed,
    Failed,
    Paused
}
