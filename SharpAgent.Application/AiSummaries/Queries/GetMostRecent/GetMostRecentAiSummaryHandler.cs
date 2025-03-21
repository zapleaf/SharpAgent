﻿using AutoMapper;
using MediatR;

using SharpAgent.Application.AiSummaries.Common;
using SharpAgent.Application.IRepositories;

namespace SharpAgent.Application.AiSummaries.Queries.GetMostRecent;

public class GetMostRecentAiSummaryHandler : IRequestHandler<GetMostRecentAiSummaryQuery, AiSummaryResponse?>
{
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly IMapper _mapper;

    public GetMostRecentAiSummaryHandler(IAiSummaryRepository aiSummaryRepository, IMapper mapper)
    {
        _aiSummaryRepository = aiSummaryRepository;
        _mapper = mapper;
    }

    public async Task<AiSummaryResponse?> Handle(GetMostRecentAiSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await _aiSummaryRepository.GetMostRecentByVideoId(request.VideoId);
        return _mapper.Map<AiSummaryResponse?>(summary);
    }
}
