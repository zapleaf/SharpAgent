using AutoMapper;

using SharpAgent.Domain.Entities;
using SharpAgent.Application.AiSummaries.Create;

namespace SharpAgent.Application.AiSummaries.Common;

public class AiSummaryMappingProfile : Profile
{
    public AiSummaryMappingProfile()
    {
        CreateMap<AiSummary, AiSummaryDto>();
        CreateMap<CreateAiSummaryCommand, AiSummary>();
    }
}
