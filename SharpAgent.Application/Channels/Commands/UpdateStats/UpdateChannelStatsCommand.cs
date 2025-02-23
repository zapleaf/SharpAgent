using MediatR;

namespace SharpAgent.Application.Channels.Commands.UpdateStats;

public class UpdateChannelStatsCommand : IRequest<bool>
{
    public Guid ChannelId { get; set; }
}
