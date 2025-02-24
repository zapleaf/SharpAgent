namespace SharpAgent.Application.PromptVersions.Common;

public class PromptVersionResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}