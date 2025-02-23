using AutoMapper;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Videos.Common;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetByChannel;

public class GetVideosByChannelHandler : IRequestHandler<GetVideosByChannelQuery, List<VideoDto>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;

    public GetVideosByChannelHandler(IVideoRepository videoRepository, IMapper mapper)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
    }

    public async Task<List<VideoDto>> Handle(GetVideosByChannelQuery request, CancellationToken cancellationToken)
    {
        var videos = await _videoRepository.GetByChannel(request.ChannelId);
        return _mapper.Map<List<VideoDto>>(videos);
    }
}
