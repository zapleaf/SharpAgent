using MediatR;

using SharpAgent.Application.AiSummaries.Common;

namespace SharpAgent.Application.AiSummaries.Queries.GetMostRecent;

public class GetMostRecentAiSummaryQuery : IRequest<AiSummaryDto?>
{
    public int VideoId { get; set; }
}
