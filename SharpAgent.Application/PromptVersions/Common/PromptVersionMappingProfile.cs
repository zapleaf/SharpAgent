using AutoMapper;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.PromptVersions.Common;

public class PromptVersionMappingProfile : Profile
{
    public PromptVersionMappingProfile()
    {
        CreateMap<PromptVersion, PromptVersionDto>();
        CreateMap<PromptVersionDto, PromptVersion>();
    }
}