using AutoMapper;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Videos.Common;
using SharpAgent.Domain.Entities;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetAll;

public class GetAllVideosHandler : IRequestHandler<GetAllVideosQuery, List<VideoResponse>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;

    public GetAllVideosHandler(IVideoRepository videoRepository, IMapper mapper)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
    }

    public async Task<List<VideoResponse>> Handle(GetAllVideosQuery request, CancellationToken cancellationToken)
    {
        var videos = await _videoRepository.GetAll();
        return _mapper.Map<List<VideoResponse>>(videos);
    }
}
