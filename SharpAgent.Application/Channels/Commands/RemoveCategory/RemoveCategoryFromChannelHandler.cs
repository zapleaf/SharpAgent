using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Channels.Commands.RemoveCategory;

public class RemoveCategoryFromChannelHandler : IRequestHandler<RemoveCategoryFromChannelCommand, bool>
{
    private readonly IChannelRepository _channelRepository;

    public RemoveCategoryFromChannelHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<bool> Handle(RemoveCategoryFromChannelCommand request, CancellationToken cancellationToken)
    {
        return await _channelRepository.RemoveCategoryFromChannel(request.CategoryId, request.ChannelId);
    }
}