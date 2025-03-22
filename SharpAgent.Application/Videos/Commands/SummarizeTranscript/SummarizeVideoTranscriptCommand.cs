using MediatR;

namespace SharpAgent.Application.Videos.Commands.SummarizeTranscript;

public class SummarizeVideoTranscriptCommand : IRequest<Guid?>
{
    public Guid AiSummaryId { get; set; }
}