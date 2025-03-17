using AutoMapper;
using SharpAgent.Application.Categories.Queries.GetAll;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.Channels.Common;

public class ChannelMappingProfile : Profile
{
    //public ChannelMappingProfile()
    //{
    //    CreateMap<Channel, ChannelResponse>()
    //        .ForMember(dest => dest.TrackedVideos,
    //            opt => opt.MapFrom(src => src.Videos.Count(v => !v.IsDeleted)))
    //        // Either provide simplified Category data or just counts
    //        .ForMember(dest => dest.Categories,
    //            opt => opt.MapFrom(src => src.Categories.Select(c => new { c.Id, c.Name })))
    //        .MaxDepth(2);

    //    CreateMap<ChannelResponse, Channel>();
    //}

    public ChannelMappingProfile()
    {
        CreateMap<Channel, ChannelResponse>()
            .ForMember(dest => dest.TrackedVideos,
                opt => opt.MapFrom(src => src.Videos.Count(v => !v.IsDeleted)))
            // Use proper CategoryResponse type instead of anonymous type
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name
                })))
            .MaxDepth(2);

        CreateMap<ChannelResponse, Channel>();
    }
}
