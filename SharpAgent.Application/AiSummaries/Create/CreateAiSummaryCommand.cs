using MediatR;

namespace SharpAgent.Application.AiSummaries.Create;

public class CreateAiSummaryCommand : IRequest<Guid>
{
    public int VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Transcript { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? TokensUsed { get; set; }
}
