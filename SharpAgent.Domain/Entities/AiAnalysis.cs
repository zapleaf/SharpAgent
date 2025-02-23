using System.ComponentModel.DataAnnotations;

using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class AiAnalysis : BaseEntity
{
    [Required]
    public int ChannelId { get; set; }
    public virtual Channel Channel { get; set; } = null!;

    [Required]
    public int PromptVersionId { get; set; }
    public virtual PromptVersion PromptVersion { get; set; } = null!;

    [Required]
    public string AnalysisType { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Analysis { get; set; } = string.Empty;

    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    public int? TokensUsed { get; set; }
}
