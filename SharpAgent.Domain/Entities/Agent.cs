using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;


/// <summary>
/// Represents a specialized AI persona with defined characteristics and responsibilities.
/// Acts as a configurable template that can be instantiated by concrete agent implementations.
/// Supports versioning and refinement of agent behaviors over time.
/// </summary>
public class Agent : BaseEntity
{
    public Agent()
    {
        Tasks = new List<AgentTask>();
        Workflows = new List<AgentWorkflow>();
    }

    public required string Name { get; set; }
    public required string Description { get; set; }

    /// <summary>
    /// Defines the agent's specialized function (e.g., Researcher, Analyst, Critic)
    /// Maps to concrete implementations in code.
    /// </summary>
    public required string Role { get; set; }

    /// <summary>
    /// Version number for this agent definition.
    /// Allows tracking changes and improvements to agent behavior over time.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Defines how the agent interacts and its communication style.
    /// Used to maintain consistent personality in language model interactions.
    /// </summary>
    public string? Personality { get; set; }

    /// <summary>
    /// Serialized array of domains this agent specializes in.
    /// Used to guide and constrain agent responses to relevant areas.
    /// </summary>
    public string? ExpertiseAreas { get; set; }

    /// <summary>
    /// Serialized array of rules and limitations that govern agent behavior.
    /// Explicitly defines what the agent can and cannot do.
    /// </summary>
    public string? Constraints { get; set; }

    /// <summary>
    /// Indicates if this agent definition is currently available for use.
    /// Inactive agents are maintained for historical reference but cannot be instantiated.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Structured JSON configuration specific to the agent's role.
    /// Maps to strongly-typed configuration classes in code.
    /// </summary>
    public string? ConfigurationJson { get; set; }

    /// <summary>
    /// Optional link to the previous version of this agent definition.
    /// Enables tracking the evolution of agent behavior over time.
    /// </summary>
    public Guid? PreviousVersionId { get; set; }

    /// <summary>
    /// Reference to the previous version of this agent definition.
    /// </summary>
    public Agent? PreviousVersion { get; set; }

    /// <summary>
    /// The primary Language Model this agent uses for reasoning and interaction.
    /// </summary>
    public Guid LanguageModelId { get; set; }
    public required LanguageModel LanguageModel { get; set; }

    public ICollection<AgentTask> Tasks { get; set; }
    public ICollection<AgentWorkflow> Workflows { get; set; }
}
