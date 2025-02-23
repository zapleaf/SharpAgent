using AutoMapper;
using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Channels.Queries.GetByCategory;

public class GetChannelsByCategoryHandler : IRequestHandler<GetChannelsByCategoryQuery, List<ChannelDto>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;

    public GetChannelsByCategoryHandler(IChannelRepository channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    public async Task<List<ChannelDto>> Handle(GetChannelsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var channels = await _channelRepository.GetByCategory(request.CategoryId);
        return _mapper.Map<List<ChannelDto>>(channels);
    }
}
