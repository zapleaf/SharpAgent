using AutoMapper;
using MediatR;

using SharpAgent.Application.AiAnalysises.Common;
using SharpAgent.Application.IRepositories;

namespace SharpAgent.Application.AiAnalysises.Queries.GetMostRecent;

public class GetMostRecentAiAnalysisHandler : IRequestHandler<GetMostRecentAiAnalysisQuery, AiAnalysisDto?>
{
    private readonly IAiAnalysisRepository _aiAnalysisRepository;
    private readonly IMapper _mapper;

    public GetMostRecentAiAnalysisHandler(IAiAnalysisRepository aiAnalysisRepository, IMapper mapper)
    {
        _aiAnalysisRepository = aiAnalysisRepository;
        _mapper = mapper;
    }

    public async Task<AiAnalysisDto?> Handle(GetMostRecentAiAnalysisQuery request, CancellationToken cancellationToken)
    {
        var analysis = await _aiAnalysisRepository.GetMostRecentByChannelId(request.ChannelId);
        return analysis != null ? _mapper.Map<AiAnalysisDto>(analysis) : null;
    }
}
