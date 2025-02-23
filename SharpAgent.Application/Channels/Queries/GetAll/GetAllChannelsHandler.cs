using AutoMapper;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Channels.Common;
using MediatR;

namespace SharpAgent.Application.Channels.Queries.GetAll;

public class GetAllChannelsHandler : IRequestHandler<GetAllChannelsQuery, List<ChannelDto>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;

    public GetAllChannelsHandler(IChannelRepository channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    public async Task<List<ChannelDto>> Handle(GetAllChannelsQuery request, CancellationToken cancellationToken)
    {
        var channels = request.IncludeVideos && request.IncludeCategories
            ? await _channelRepository.GetWithCategoriesAndVideos()
            : request.IncludeCategories
                ? await _channelRepository.GetWithCategories()
                : await _channelRepository.GetAll();

        return _mapper.Map<List<ChannelDto>>(channels);
    }
}
