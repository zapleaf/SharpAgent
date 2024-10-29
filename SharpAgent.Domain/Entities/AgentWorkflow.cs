using SharpAgent.Domain.Common;
using SharpAgent.Domain.Enums;

namespace SharpAgent.Domain.Entities;

/// <summary>
/// Orchestrates collaboration between multiple agents to accomplish complex tasks.
/// Workflows define the sequence and dependencies of agent tasks.
/// Maintains execution state and history for the overall process.
/// </summary>
public class AgentWorkflow : BaseEntity
{
    public AgentWorkflow()
    {
        Agents = new List<Agent>();
        Tasks = new List<AgentTask>();
    }

    public required string Name { get; set; }
    public required string Description { get; set; }

    /// <summary>
    /// Version number for this workflow definition.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Structured JSON defining the sequence and dependencies of workflow steps.
    /// Maps to concrete workflow definitions in code.
    /// </summary>
    public string? WorkflowSteps { get; set; }

    /// <summary>
    /// Current state of the workflow execution.
    /// </summary>
    public WorkflowStatus Status { get; set; }

    /// <summary>
    /// Index of the current step in workflow execution.
    /// </summary>
    public int CurrentStep { get; set; }

    /// <summary>
    /// Start time of workflow execution.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Completion time of workflow execution.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Optional error details if workflow execution failed.
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// Agents participating in this workflow.
    /// </summary>
    public ICollection<Agent> Agents { get; set; }

    /// <summary>
    /// Individual tasks that make up this workflow.
    /// </summary>
    public ICollection<AgentTask> Tasks { get; set; }

    /// <summary>
    /// Optional link to the previous version of this workflow.
    /// </summary>
    public Guid? PreviousVersionId { get; set; }
    public AgentWorkflow? PreviousVersion { get; set; }
}
