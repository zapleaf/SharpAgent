using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

/// <summary>
/// Represents a specific operation performed by an agent using available services.
/// Tasks are the building blocks of workflows and track individual agent contributions.
/// Maintains execution history and results for auditing and improvement.
/// </summary>
public class AgentTask : BaseEntity
{
    public Guid AgentId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    /// <summary>
    /// Identifies which service this task uses (Vector, Embedding, etc.)
    /// Maps to concrete service interfaces in code.
    /// </summary>
    public required string ServiceType { get; set; }

    /// <summary>
    /// Structured JSON input parameters required by the service.
    /// Maps to strongly-typed request objects in code.
    /// </summary>
    public string? InputParameters { get; set; }

    /// <summary>
    /// Structured JSON schema defining the expected result format.
    /// Used for validating service responses.
    /// </summary>
    public string? OutputSchema { get; set; }

    /// <summary>
    /// Current state of the task execution.
    /// </summary>
    public TaskStatus Status { get; set; }

    /// <summary>
    /// Stores the task's execution results in structured JSON format.
    /// Maps to strongly-typed response objects in code.
    /// </summary>
    public string? ResultJson { get; set; }

    /// <summary>
    /// Optional error details if task execution failed.
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// Execution duration in milliseconds.
    /// Used for performance tracking and optimization.
    /// </summary>
    public long? ExecutionTimeMs { get; set; }

    public required Agent Agent { get; set; }
    public Guid? WorkflowId { get; set; }
    public AgentWorkflow? Workflow { get; set; }
}
