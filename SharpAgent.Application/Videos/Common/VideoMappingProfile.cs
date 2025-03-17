using AutoMapper;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.Videos.Common;

public class VideoMappingProfile : Profile
{
    public VideoMappingProfile()
    {
        CreateMap<Video, VideoResponse>()
            .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.Channel.Id))
            .ForMember(dest => dest.ChannelTitle, opt => opt.MapFrom(src => src.Channel.Title))
            .ForMember(dest => dest.ChannelSubscriberCount, opt => opt.MapFrom(src => src.Channel.SubscriberCount))
            .MaxDepth(2);

        CreateMap<VideoResponse, Video>();
    }
}