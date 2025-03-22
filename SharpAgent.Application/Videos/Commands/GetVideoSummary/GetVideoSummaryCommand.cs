using MediatR;
using SharpAgent.Application.AiSummaries.Common;

namespace SharpAgent.Application.Videos.Commands.GetSummary;

public class GetVideoSummaryCommand : IRequest<AiSummaryResponse>
{
    public Guid VideoId { get; set; }
}