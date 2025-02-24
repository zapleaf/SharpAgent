using AutoMapper;
using MediatR;

using SharpAgent.Application.AiSummaries.Common;
using SharpAgent.Application.IRepositories;

namespace SharpAgent.Application.AiSummaries.Queries.GetByVideo;

public class GetAiSummariesByVideoHandler : IRequestHandler<GetAiSummariesByVideoQuery, List<AiSummaryResponse>>
{
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly IMapper _mapper;

    public GetAiSummariesByVideoHandler(IAiSummaryRepository aiSummaryRepository, IMapper mapper)
    {
        _aiSummaryRepository = aiSummaryRepository;
        _mapper = mapper;
    }

    public async Task<List<AiSummaryResponse>> Handle(GetAiSummariesByVideoQuery request, CancellationToken cancellationToken)
    {
        var summaries = await _aiSummaryRepository.GetByVideoId(request.VideoId);
        return _mapper.Map<List<AiSummaryResponse>>(summaries);
    }
}
