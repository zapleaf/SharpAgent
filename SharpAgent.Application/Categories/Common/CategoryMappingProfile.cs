using AutoMapper;
using SharpAgent.Application.Categories.Commands.Create;
using SharpAgent.Application.Categories.Queries.GetAll;
using SharpAgent.Application.Categories.Queries.GetById;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.Categories.Common;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.ChannelCount,
                      opt => opt.MapFrom(src => src.Channels.Count));

        CreateMap<CreateCategoryRequest, Category>();

        CreateMap<Category, CategoryDetailsResponse>();

        CreateMap<Channel, CategoryChannelResponse>();
    }
}
