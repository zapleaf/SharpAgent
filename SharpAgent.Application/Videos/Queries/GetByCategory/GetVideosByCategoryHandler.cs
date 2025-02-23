using AutoMapper;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Videos.Common;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetByCategory;

public class GetVideosByCategoryHandler : IRequestHandler<GetVideosByCategoryQuery, List<VideoDto>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;

    public GetVideosByCategoryHandler(IVideoRepository videoRepository, IMapper mapper)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
    }

    public async Task<List<VideoDto>> Handle(GetVideosByCategoryQuery request, CancellationToken cancellationToken)
    {
        var videos = await _videoRepository.GetByCategory(request.CategoryId);
        return _mapper.Map<List<VideoDto>>(videos);
    }
}