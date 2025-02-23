using MediatR;

namespace SharpAgent.Application.Channels.Commands.Delete;

public class DeleteChannelCommand : IRequest<bool>
{
    public Guid ChannelId { get; set; }
}
