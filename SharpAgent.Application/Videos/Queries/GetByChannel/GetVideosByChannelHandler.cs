using AutoMapper;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Videos.Common;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetByChannel;

public class GetVideosByChannelHandler : IRequestHandler<GetVideosByChannelQuery, List<VideoResponse>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;

    public GetVideosByChannelHandler(IVideoRepository videoRepository, IMapper mapper)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
    }

    public async Task<List<VideoResponse>> Handle(GetVideosByChannelQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var videos = await _videoRepository.GetByChannel(request.ChannelId);
            var response = _mapper.Map<List<VideoResponse>>(videos);
            return response;
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
        }

        return null;
    }
}
