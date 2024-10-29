using SharpAgent.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Domain.Entities;

/// <summary>
/// Represents a Language Model that can be used by agents for tasks like 
/// generation, reasoning, and conversation
/// </summary>
public class LanguageModel : BaseEntity
{
    public LanguageModel()
    {
        Agents = new List<Agent>();
    }

    public required string Name { get; set; }
    public required string Provider { get; set; }  // e.g., OpenAI, Anthropic, etc.
    public string? Description { get; set; }

    /// <summary>
    /// The specific model identifier from the provider (e.g., gpt-4, claude-3-opus)
    /// </summary>
    public required string ModelIdentifier { get; set; }

    /// <summary>
    /// Maximum context length the model can handle
    /// </summary>
    public int MaxTokens { get; set; }

    /// <summary>
    /// Indicates if this model is currently available for use
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// JSON string containing model-specific configuration and capabilities
    /// </summary>
    public string? CapabilitiesJson { get; set; }

    // Navigation property
    public ICollection<Agent> Agents { get; set; }
}
