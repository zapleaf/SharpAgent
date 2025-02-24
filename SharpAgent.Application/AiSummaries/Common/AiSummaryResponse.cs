namespace SharpAgent.Application.AiSummaries.Common;

public class AiSummaryResponse
{
    public int Id { get; set; }
    public int VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Transcript { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? TokensUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}
