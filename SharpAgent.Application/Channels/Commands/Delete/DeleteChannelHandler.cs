using MediatR;
using SharpAgent.Application.IRepositories;

namespace SharpAgent.Application.Channels.Commands.Delete;

public class DeleteChannelHandler : IRequestHandler<DeleteChannelCommand, bool>
{
    private readonly IChannelRepository _channelRepository;

    public DeleteChannelHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<bool> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        return await _channelRepository.Delete(request.ChannelId);
    }
}
