using MediatR;

using SharpAgent.Application.AiAnalysises.Common;

namespace SharpAgent.Application.AiAnalysises.Queries.GetByChannel;

public class GetAiAnalysesByChannelQuery : IRequest<List<AiAnalysisDto>>
{
    public Guid ChannelId { get; set; }
}
