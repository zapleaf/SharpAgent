using MediatR;

using SharpAgent.Application.AiAnalysises.Common;

namespace SharpAgent.Application.AiAnalysises.Queries.GetMostRecent;

public class GetMostRecentAiAnalysisQuery : IRequest<AiAnalysisResponse?>
{
    public Guid ChannelId { get; set; }
}
