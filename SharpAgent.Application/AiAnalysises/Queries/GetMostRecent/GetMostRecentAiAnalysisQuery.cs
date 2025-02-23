using MediatR;

using SharpAgent.Application.AiAnalysises.Common;

namespace SharpAgent.Application.AiAnalysises.Queries.GetMostRecent;

public class GetMostRecentAiAnalysisQuery : IRequest<AiAnalysisDto?>
{
    public Guid ChannelId { get; set; }
}
