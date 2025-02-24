using AutoMapper;
using MediatR;

using SharpAgent.Application.AiAnalysises.Common;
using SharpAgent.Application.IRepositories;

namespace SharpAgent.Application.AiAnalysises.Queries.GetByChannel;

public class GetAiAnalysesByChannelHandler : IRequestHandler<GetAiAnalysesByChannelQuery, List<AiAnalysisResponse>>
{
    private readonly IAiAnalysisRepository _aiAnalysisRepository;
    private readonly IMapper _mapper;

    public GetAiAnalysesByChannelHandler(IAiAnalysisRepository aiAnalysisRepository, IMapper mapper)
    {
        _aiAnalysisRepository = aiAnalysisRepository;
        _mapper = mapper;
    }

    public async Task<List<AiAnalysisResponse>> Handle(GetAiAnalysesByChannelQuery request, CancellationToken cancellationToken)
    {
        var analyses = await _aiAnalysisRepository.GetByChannelId(request.ChannelId);
        return _mapper.Map<List<AiAnalysisResponse>>(analyses);
    }
}
