using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Channels.Commands.UpdateNotes;

public class UpdateChannelNotesHandler : IRequestHandler<UpdateChannelNotesCommand, bool>
{
    private readonly IChannelRepository _channelRepository;

    public UpdateChannelNotesHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<bool> Handle(UpdateChannelNotesCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.Get(request.ChannelId);
        if (channel == null)
            return false;

        channel.Notes = request.Notes;
        await _channelRepository.Update(channel);
        return true;
    }
}