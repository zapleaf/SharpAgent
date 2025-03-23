using AutoMapper;

using SharpAgent.Domain.Entities;
using SharpAgent.Application.AiSummaries.Create;
using SharpAgent.Application.AiSummaries.Queries.GetMostRecent;

namespace SharpAgent.Application.AiSummaries.Common;

public class AiSummaryMappingProfile : Profile
{
    public AiSummaryMappingProfile()
    {
        CreateMap<AiSummary, AiSummaryResponse>();
        CreateMap<CreateAiSummaryCommand, AiSummary>();
        CreateMap<GetMostRecentAiSummaryQuery, AiSummary>();
    }
}
