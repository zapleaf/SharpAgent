using MediatR;

using SharpAgent.Application.AiSummaries.Common;

namespace SharpAgent.Application.AiSummaries.Queries.GetByVideo;

public class GetAiSummariesByVideoQuery : IRequest<List<AiSummaryResponse>>
{
    public Guid VideoId { get; set; }
}
