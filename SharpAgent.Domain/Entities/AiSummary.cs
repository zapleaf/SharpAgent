using System.ComponentModel.DataAnnotations;

using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;
public class AiSummary : BaseEntity
{
    [Required]
    public Guid VideoId { get; set; }
    public virtual Video Video { get; set; } = null!;

    [Required]
    public Guid PromptVersionId { get; set; }
    public virtual PromptVersion PromptVersion { get; set; } = null!;

    [Required]
    public string Summary { get; set; } = string.Empty;

    [Required]
    public string Transcript { get; set; } = string.Empty;

    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    public int? TokensUsed { get; set; }
}
