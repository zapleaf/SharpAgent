using AutoMapper;
using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Channels.Queries.GetById;

public class GetChannelByIdHandler : IRequestHandler<GetChannelByIdQuery, ChannelDto>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;

    public GetChannelByIdHandler(IChannelRepository channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    public async Task<ChannelDto> Handle(GetChannelByIdQuery request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.Get(request.Id);
        return _mapper.Map<ChannelDto>(channel);
    }
}
