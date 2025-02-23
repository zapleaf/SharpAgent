using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Channels.Commands.AddCategory;

public class AddCategoryToChannelHandler : IRequestHandler<AddCategoryToChannelCommand, bool>
{
    private readonly IChannelRepository _channelRepository;

    public AddCategoryToChannelHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<bool> Handle(AddCategoryToChannelCommand request, CancellationToken cancellationToken)
    {
        return await _channelRepository.AddCategoryToChannel(request.CategoryId, request.ChannelId);
    }
}
